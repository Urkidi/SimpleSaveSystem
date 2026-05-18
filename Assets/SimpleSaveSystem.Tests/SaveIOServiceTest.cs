using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using R3;
using SimpleSaveSystem.Core;
using SimpleSaveSystem.Core.Data;
using SimpleSaveSystem.Core.Services;
using UnityEngine;
using UnityEngine.TestTools;

namespace SimpleSaveSystem.Tests
{
    [TestFixture]
    public class SaveIOServiceTest
    {
        private static string _defaultValue = "default";
        
        private Mock<IDataReadService> _mockDataReadService;
        private Mock<IDataWriteService> _mockDataWriteService;
        private Mock<ISerializationService> _mockSerializationService;
        private Mock<IDefaultSaveProvider<FakeSaveData>> _stubDefaultSaveProvider;
        private Mock<IEncryptionService> _mockEncryptionService;
        private Mock<IHashService> _mockHashService;
        private Mock<IUriProvider> _stubUriProvider;
        private Mock<ISaveVersionProvider> _stubSaveVersionProvider;

        private byte[] _dataReadOutput = { };
        private byte[] _serializedOutput = { };

        private byte[] _encryptedOutput = { };
        private string _hashedOutput = "";
        private FakeSaveData _saveData = new();
        private FakeSaveData _defaultSaveData = new(){ Value = _defaultValue};

        private byte[] _serializedIndexOutput = { };
        private SaveIndexData _saveIndexData = new();

        private string _version = "1";
        private string _metaUri = "C://uripath/metauri.json";
        private string _uri = "C://uripath/";
        private string _uriWithSlot = "C://uripath/slot";
        private string _lastSaveId = "0";
        private string _nonExistingId = "-1";
        
        [SetUp]
        public void Setup()
        {
            _mockDataReadService = new Mock<IDataReadService>();
            _mockDataWriteService = new Mock<IDataWriteService>();
            _mockSerializationService = new Mock<ISerializationService>();
            _mockEncryptionService = new Mock<IEncryptionService>();
            _mockHashService = new Mock<IHashService>();
            _stubUriProvider = new Mock<IUriProvider>();
            _stubSaveVersionProvider = new Mock<ISaveVersionProvider>();
            _stubDefaultSaveProvider = new Mock<IDefaultSaveProvider<FakeSaveData>>();

            _mockDataWriteService.Setup(service => service.WriteData(It.IsAny<string>(), It.IsAny<byte[]>()));

            _mockSerializationService.Setup(service => service.Serialize(It.IsAny<FakeSaveData>()))
                .Returns(_serializedOutput);
            _mockSerializationService.Setup(service => service.Deserialize<FakeSaveData>(It.IsAny<byte[]>()))
                .Returns(_saveData);

            _mockSerializationService.Setup(service => service.Serialize(It.IsAny<SaveIndexData>()))
                .Returns(_serializedIndexOutput);
            _mockSerializationService.Setup(service => service.Deserialize<SaveIndexData>(It.IsAny<byte[]>()))
                .Returns(_saveIndexData);

            _mockEncryptionService.Setup(service => service.EncryptData(It.IsAny<byte[]>())).Returns(_encryptedOutput);
            _mockEncryptionService.Setup(service => service.EncryptData(It.IsAny<byte[]>())).Returns(_encryptedOutput);

            _mockHashService.Setup(service => service.GetHash(It.IsAny<byte[]>())).Returns(_hashedOutput);

            _stubSaveVersionProvider.SetupGet(provider => provider.Version).Returns(_version);
            _stubUriProvider.SetupGet(provider => provider.SaveDataUri).Returns(_uri);
            _stubUriProvider.SetupGet(provider => provider.MetaDataUri).Returns(_metaUri);
            _stubUriProvider.Setup(provider => provider.GetSlotUri(It.IsAny<string>())).Returns(_uriWithSlot);
            _stubDefaultSaveProvider.Setup(provider => provider.CreateSave()).Returns(_defaultSaveData);
        }

        [Test]
        public void CreateSaveService_NoIndexExists_CreatesIndexSave()
        {
            SetUpReadService(false);
            
            var saveSystem = CreateSaveSystem();

            _mockDataReadService.Verify(service => service.TryRead(It.IsAny<string>(), out _dataReadOutput),
                Times.Once);
            Assert.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void CreateSaveService_IndexExists_LoadsIndexSave(int amountOfSaves)
        {
            SetUpIndexData(amountOfSaves, _lastSaveId);
            
            SetUpReadService(true);
            
            var saveSystem = CreateSaveSystem();
            
            _mockDataReadService.Verify(service => service.TryRead(It.IsAny<string>(), out _dataReadOutput),
                Times.Once);
            Assert.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(amountOfSaves));
        }

        [Test]
        public void TryLoadCreate_NoSaveExists_CreatesSave()
        {
            SetUpIndexData(0, _lastSaveId);
            
            SetUpReadService(false);
            var saveSystem = CreateSaveSystem();
            
            Assert.That(saveSystem.TryLoadCreate("", out var saveData), Is.True);
            Assert.That(saveData.Value, Is.EqualTo(_defaultValue));
        }

        [Test]
        public void TryLoad_NoSaveExists_PrintsError()
        {
            SetUpIndexData(0, _lastSaveId);
            
            SetUpReadService(false);
            var saveSystem = CreateSaveSystem();
            
            LogAssert.Expect(LogType.Error, String.Format(SaveErrorMessage.UnableToReadPath, _uriWithSlot));
            Assert.That(saveSystem.TryLoad("", out var saveData), Is.False);
        }

        [Test]
        public void TryLoad_CannotVerifyHash_PrintsError()
        {
            SetUpIndexData(1, _lastSaveId);
            
            SetUpReadService(true);
            SetUpHashService(false);
            var saveSystem = CreateSaveSystem();
            
            LogAssert.Expect(LogType.Error, SaveErrorMessage.HashDoesntMatch);
            Assert.That(saveSystem.TryLoad(_lastSaveId, out var saveData), Is.False);
        }

        [Test]
        public void TryLoadCreate_NoSaveExists_CreatesIndexData()
        {
            SetUpIndexData(0, "");
            
            SetUpReadService(false);
            var saveSystem = CreateSaveSystem();
            
            Assume.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(0));
            
            Assert.That(saveSystem.TryLoadCreate(_lastSaveId, out var data), Is.True);
            Assert.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(1));
            Assert.That(saveSystem.SaveSlotIds.First(), Is.EqualTo(_lastSaveId));
        }

        [Test]
        public void TryLoad_SaveExists_DoesNotUpdateIndex()
        {
            SetUpIndexData(1, _lastSaveId);
            
            SetUpReadService(true);
            SetUpHashService(true);
            var saveSystem = CreateSaveSystem();
            
            Assume.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(1));
            
            Assert.That(saveSystem.TryLoad(_lastSaveId, out var data), Is.True);
            Assert.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(1));
            _mockDataWriteService.Verify(service => service.WriteData(_metaUri, It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        public void TrySave_NoSaveIdExists_PrintsError()
        {
            SetUpIndexData(1, _lastSaveId);
            
            SetUpReadService(true);
            var saveSystem = CreateSaveSystem();
            
            LogAssert.Expect(LogType.Error, String.Format(SaveErrorMessage.UnableToFindSaveWithId, _nonExistingId));
            Assert.That(saveSystem.TrySave(_nonExistingId, CreateSave("s")), Is.False);
        }

        [Test]
        public void TrySave_ExistingSaveId_WritesSave()
        {
            SetUpIndexData(1, _lastSaveId);
            
            SetUpReadService(true);
            var saveSystem = CreateSaveSystem();
            
            Assert.That(saveSystem.TrySave(_lastSaveId, CreateSave("s")), Is.True);
            _mockDataWriteService.Verify(service => service.WriteData(_uriWithSlot, It.IsAny<byte[]>()), Times.Once);
        }

        [Test]
        public void TrySave_NonExistingSaveId_DoesNotUpdateIndexData()
        {
            SetUpIndexData(1, _lastSaveId);
            
            SetUpReadService(true);
            var saveSystem = CreateSaveSystem();
            var data = CreateSave("data");
            
            Assume.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(1));

            LogAssert.Expect(LogType.Error, String.Format(SaveErrorMessage.UnableToFindSaveWithId, _nonExistingId));
            Assert.That(saveSystem.TrySave(_nonExistingId, data), Is.False);
            _mockDataWriteService.Verify(service => service.WriteData(_metaUri, It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        public void TrySave_ExistingSaveId_UpdatesIndexData()
        {
            SetUpIndexData(1, _lastSaveId);
            
            SetUpReadService(true);
            var saveSystem = CreateSaveSystem();
            var data = CreateSave("data");
            
            Assume.That(saveSystem.SaveSlotIds.Count, Is.EqualTo(1));

            Assert.That(saveSystem.TrySave(_lastSaveId, data), Is.True);
            _mockDataWriteService.Verify(service => service.WriteData(_metaUri, It.IsAny<byte[]>()), Times.Once);
        }
        
        [Test]
        public void TrySave_ExistingSaveId_UpdatesIndexLastSaveId()
        {
            SetUpIndexData(1, _nonExistingId);
            
            SetUpReadService(true);
            var saveSystem = CreateSaveSystem();
            var data = CreateSave("data");
            
            Assume.That(saveSystem.LastSavedId, Is.EqualTo(_nonExistingId));

            Assert.That(saveSystem.TrySave(_lastSaveId, data), Is.True);
            Assert.That(saveSystem.LastSavedId, Is.EqualTo(_lastSaveId));
        }
        
        private void SetUpReadService(bool canRead)
        {
            _mockDataReadService.Setup(service => service.TryRead(It.IsAny<string>(), out _dataReadOutput))
                .Returns(canRead);
        }

        private void SetUpHashService(bool canVerify)
        {
            _mockHashService.Setup(service => service.VerifyHash(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(canVerify);
        }

        private SaveIOService<FakeSaveData> CreateSaveSystem()
        {
            return new SaveIOService<FakeSaveData>(_mockDataReadService.Object, _mockDataWriteService.Object,
                _mockSerializationService.Object, _mockEncryptionService.Object, _mockHashService.Object,
                _stubUriProvider.Object, _stubSaveVersionProvider.Object, _stubDefaultSaveProvider.Object);
        }

        private void SetUpIndexData(int entries, string lastSavedSlotId)
        {

            var indexData = new SaveIndexData()
            {
                LastSavedSlotId = lastSavedSlotId,
                SaveSlots = new List<SaveSlotMetaData>()
                
            };
            
            for (int i = 0; i < entries; i++)
            {
                indexData.SaveSlots.Add(new SaveSlotMetaData()
                {
                    Id = i.ToString(),
                    Hash = "10",
                    LastModified = DateTime.UtcNow,
                    Version = _version
                });    
            }

            _saveIndexData = indexData;
            
            _mockSerializationService.Setup(service => service.Deserialize<SaveIndexData>(It.IsAny<byte[]>()))
                .Returns(_saveIndexData);
        }

        private FakeSaveData CreateSave(string value)
        {
            return new FakeSaveData()
            {
                Value = value
            };
        }
    }
}
# Simple Save System

---

A lightweight save system for serializable data.

### Features:
- Serialization
- Encryption support
- Hash validation
- Save versioning
- Custom save IDs


## Usage
Create a Serializable class to hold all of your save data.
 
```csharp
[Serializable]
public class MainSave
{
    public int Level;
    public int Currency;
}
```
Create an instance of SaveSystem of the type created above.
```csharp
ISaveSystem<MainSave> saveSystem;
```
When saving and loading, do it through the reference to ISaveSystem

Currently all ids for the savegames are sequential numbers in string format unless specified otherwise."0", "1", "2"

When the SaveSystem is created, the last saved id available.

This package requires you to implement the following interfaces.
- IEncryptionService
- IHashService
- ISerializationService
- IDataReadService
- IDataWriteService
- ISaveVersionProvider
- IDefaultSaveProvider< T >

## Package URLs

#### Core package
```https://github.com/Urkidi/SimpleSaveSystem.git?path=/Assets/SimpleSaveSystem.Core/```

#### Defaults package
```https://github.com/Urkidi/SimpleSaveSystem.git?path=/Assets/SimpleSaveSystem.Defaults/```

#### Example implementation package

```https://github.com/Urkidi/SimpleSaveSystem.git?path=/Assets/SimpleSaveSystem.Example/```
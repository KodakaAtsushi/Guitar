using Cysharp.Threading.Tasks;

internal interface ILoader
{
    internal UniTask Load(object option = null);
    internal UniTask Unload();
}
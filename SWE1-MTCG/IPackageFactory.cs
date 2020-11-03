using System.Collections;

namespace SWE1_MTCG
{
    public interface IPackageFactory
    {
        IPackage CreatePackage();
        IPackage CreateHighRarityPackage();
    }
}
using System.Collections.Generic;

namespace SWE1_MTCG
{
    public interface IPackageRepository
    {
        int CreatePackage(List<ACard> cards);
        int OpenPackage(User user);
        int DeletePackage(int id);
    }
}
using System.Collections.Generic;
using SWE1_MTCG.DTOs;

namespace SWE1_MTCG.DBFeature
{
    public interface IPackageRepository
    {
        int CreatePackage(List<ACard> cards);
        int OpenPackage(User user);
        int DeletePackage(int id);
    }
}
using tcsoft_pingpongclub.Models;

namespace tcsoft_pingpongclub.Service
{
    public interface IsAuthorized
    {
        bool hasPer(int ? idRole,string url);
    }
    public class AuthorizationService : IsAuthorized
    {
        private readonly ThuctapKtktcn2024Context _context;

        public AuthorizationService(ThuctapKtktcn2024Context context)
        {
            _context = context;
        }
        public bool hasPer(int ?idRole,string urlWeb)
        {
            var urls = (from r in _context.Roles
                          join pr in _context.PermissionRoles on r.IdRole equals pr.IdRole
                          join p in _context.Permissions on pr.IdPermission equals p.IdPermission
                          where r.IdRole == idRole && r.Status == true
                                && pr.Status == true && p.Status == true
                          select p.Url).Distinct();
            if ( !urls.Contains(urlWeb)||!urls.Any())
                return false;
            return true;
        }
    }
}

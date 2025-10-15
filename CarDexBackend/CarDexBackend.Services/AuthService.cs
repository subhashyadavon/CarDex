using CarDexBackend.Shared.Dtos.Requests;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexBackend.Services;

public class AuthService: IAuthService
{
    // private readonly DatabaseContext _db;

    // public AuthService(DatabaseContext db)
    // {
    //     _db = db;
    // }

    public async Task<UserResponse> Register(RegisterRequest request)
    {
        // database stuff,
        //  - insert user into db
        //  - hash password
        //  - other?
        return new UserResponse { };
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        // database stuff,
        //  - lookup user table
        //  - verify password
        //  - issue valid token on successful credential check
        return new LoginResponse { };
    }

    public async Task Logout(Guid userId)
    {
        // database stuff,
        //  - remove token
    }
}
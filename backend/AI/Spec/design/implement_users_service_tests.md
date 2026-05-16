# Design Specification - UsersService Tests

## 1. Overview
Implementation of unit tests for `UsersService`, specifically focusing on `PostCreateUserAsync` and `PostCreateUserSsoAsync`.

## 2. Approach
Since dependencies like `UserModel` and `CryptoPasswordService` are concrete classes without virtual methods, I will:
- Use `Moq.EntityFrameworkCore` to mock the `DbSet`s within `AuthOrigin` and `AuthReplica`.
- Use the real `UserModel` and `UserProviderSsoModel` but injected with mocked `DbContext`s.
- For `CryptoPasswordService`, use the real implementation since it's a stateless utility, ensuring environment variables are set.
- Use `NSubstitute` for any interfaces that might be encountered (like `IConfiguration`).

## 3. Test Cases for `PostCreateUserAsync`
- **Success**: Valid request, email doesn't exist. Expect 201 Created.
- **Validation Error**: Invalid request (e.g., empty email). Expect 400 Bad Request with error list.
- **Duplicate Email**: Email already exists in DB. Expect 400 Bad Request with specific error message.

## 4. Test Cases for `PostCreateUserSsoAsync`
- **Success**: New user, valid provider.
- **User Exists**: Provider/Sub already exists.
- **Invalid Provider**: Provider not found or inactive.

## 5. Files to Create
- `NievoEasyFin.Tests/Mocks/Fakers/UserEntityFaker.cs`
- `NievoEasyFin.Tests/Mocks/Fakers/PostCreateUserRequestFaker.cs`
- `NievoEasyFin.Tests/Application/Services/Base/UsersServiceTests.cs`
- `NievoEasyFin.Tests/Mocks/Database/DbContextMockFactory.cs` (Helper to create mocked contexts)

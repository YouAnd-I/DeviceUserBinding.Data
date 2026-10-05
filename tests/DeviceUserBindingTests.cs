using Device.Data;
using User.Data;
using Xunit;

namespace DeviceUserBinding.Data.Tests;

public class DeviceUserBindingTests
{
    [Fact]
    public void Binding_PairsDeviceAndUser()
    {
        var device = DeviceId.Create();
        var user = UserId.Create();

        var binding = new DeviceUserBinding(device, user);

        Assert.Equal(device, binding.Device);
        Assert.Equal(user, binding.User);
    }
}

public class DeviceUserResolverTests
{
    private sealed class StubResolver(DeviceUserBinding? binding) : IDeviceUserResolver
    {
        public ValueTask<UserId?> FindUserAsync(
            DeviceId device,
            CancellationToken cancellationToken = default) =>
            new(binding is { } b && b.Device == device ? b.User : null);
    }

    [Fact]
    public async Task FindUser_ReturnsBoundUser()
    {
        var binding = new DeviceUserBinding(DeviceId.Create(), UserId.Create());
        IDeviceUserResolver resolver = new StubResolver(binding);

        Assert.Equal(binding.User, await resolver.FindUserAsync(binding.Device));
    }

    [Fact]
    public async Task FindUser_ReturnsNull_ForUnboundDevice()
    {
        var binding = new DeviceUserBinding(DeviceId.Create(), UserId.Create());
        IDeviceUserResolver resolver = new StubResolver(binding);

        Assert.Null(await resolver.FindUserAsync(DeviceId.Create()));
    }
}

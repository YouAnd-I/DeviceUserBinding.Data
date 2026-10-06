using Device.Data;
using Identity.Data;

namespace DeviceUserBinding.Data;

public readonly record struct DeviceUserBinding(DeviceId Device, UserId User);

public interface IDeviceUserResolver
{
    ValueTask<UserId?> FindUserAsync(DeviceId device, CancellationToken cancellationToken = default);
}

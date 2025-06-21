namespace DotNetEcosystemStudy.Settings.Interfaces;

public interface IDistributedIpRateLimitingSettings
{
    string RedisConnectionString { get; set; }
    bool Enabled { get; set; }

    /// <summary>
    /// Maximum number of Redis timeouts that can be experienced within the sliding timeout
    /// window before IP rate limiting is temporarily disabled.
    /// TODO: Determine/discuss a suitable maximum
    /// </summary>
    int MaxRedisTimeoutsThreshold { get; set; }

    /// <summary>
    /// Length of the sliding window in seconds to track Redis timeout exceptions.
    /// TODO: Determine/discuss a suitable sliding window
    /// </summary>
    int SlidingWindowSeconds { get; set; }
}
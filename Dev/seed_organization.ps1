param(
    [string]$Url = "http://localhost:5127/organization/organization"
)

# Arrays de nomes e segredos
$organizationNames = @(
    "Google", "Amazon", "Apple", "Microsoft", "Meta", "Nvidia", "Netflix", "Intel", "IBM", "Oracle",
    "Salesforce", "Adobe", "Cisco", "Uber", "Airbnb", "Spotify", "Snap", "Zoom", "Dropbox", "Slack",
    "Twitter", "Reddit", "TikTok", "Alibaba", "Tencent", "Baidu", "Huawei", "Samsung", "LG", "Sony",
    "ASML", "TSMC", "Qualcomm", "AMD", "Dell", "HP", "Lenovo", "Logitech", "Square", "Stripe",
    "Shopify", "Palantir", "Twilio", "Atlassian", "Datadog", "Cloudflare", "Snowflake", "MongoDB", "GitLab",
    "GitHub", "DigitalOcean", "Elastic", "Okta", "Zscaler", "CrowdStrike", "UiPath", "Roku", "Pinterest", "ByteDance",
    "Yandex", "Grab", "GoTo", "MercadoLibre", "Nubank", "Kaspersky", "Bitdefender", "Canonical", "Red Hat", "OpenAI",
    "DeepMind", "Unity", "Epic Games", "Roblox", "Niantic", "Valve", "Electronic Arts", "Activision Blizzard", "Capcom", "Bandai Namco",
    "SEGA", "ARM", "Micron", "Western Digital", "Seagate", "Garmin", "Fitbit", "Razer", "Alienware", "Anker",
    "OnePlus", "Realme", "Vivo", "Oppo", "Xiaomi", "Honor", "Zebra Technologies", "Infineon", "Keysight", "Skyworks",
    "Lattice Semiconductor", "Marvell", "Synopsys", "Cadence", "Honeywell", "Bosch", "Siemens", "Panasonic", "Philips", "3M"
)

$secrets = @(
    "keep-apple-united-strawberry", "blue-wolf-dream-orchid", "silent-moon-lake-honey", "whisper-cactus-river-shadow",
    "happy-tiger-cloud-velvet", "fast-rain-glass-ember", "lucky-fire-sky-jungle", "calm-owl-echo-peach",
    "brave-sun-leaf-crystal", "quiet-stone-spark-forest", "cool-fox-ice-plum", "wild-heart-sand-lotus",
    "lunar-rose-wave-mint", "tiny-frost-wood-petal", "sharp-wind-sea-feather", "magic-tree-flame-grape",
    "bold-snow-moss-bubble", "smooth-bird-hill-sapphire", "fresh-dust-pine-moonlight", "crazy-fish-honeydew-glow",
    "soft-wolf-bean-coral", "amber-moon-drum-flower", "loud-breeze-rock-papaya", "swift-sunbeam-fern-cookie",
    "misty-hawk-bark-sunset", "silver-lion-mint-blossom", "sunny-bird-star-mango", "tiny-leaf-sparkle-fog",
    "wise-owl-shore-vanilla", "shy-moonlight-fig-flower", "cosmic-deer-pool-apricot", "brisk-echo-apple-canyon",
    "electric-river-cloud-melon", "lucky-hill-bird-citrus", "yellow-wind-dune-maple", "mellow-star-sand-pebble",
    "sharp-ocean-pineapple-spark", "happy-dragon-berry-lemon", "polar-bear-sky-ripple", "swift-echo-plum-tornado",
    "green-wolf-frost-pear", "sunset-breeze-glass-orchid", "violet-fire-leaf-snow", "cozy-honey-fern-shadow",
    "bold-fish-rose-glimmer", "white-tiger-sun-fluff", "gentle-sea-mint-mirage", "fluffy-cloud-moon-rain",
    "quiet-glow-dust-breeze", "silent-mint-petal-thunder", "sunny-glass-fox-feather"
)

function Add-RandomOrganizations {
    param(
        [Parameter(Mandatory=$true)]
        [int]$Count
    )

    for ($i = 1; $i -le $Count; $i++) {
        $OrganizationName = Get-Random -InputObject $organizationNames
        $Secret = Get-Random -InputObject $secrets
        $ContributorsCount = Get-Random -Minimum 1000 -Maximum 10000

        $body = @{
            organizationName = $OrganizationName
            contributorsCount = $ContributorsCount
            secret = $Secret
        } | ConvertTo-Json

        
        if ($i % 1 -eq 0) {
            Write-Host "`n[$i/$Count] Enviando requisição POST para $Url..."
        }

        try {
            $response = Invoke-RestMethod -Uri $Url -Method Post -ContentType "application/json" -Body $body
        } catch {
            Write-Host "Erro ao enviar requisição: $_"
        }
    }
}

# Exemplo de uso:
# Add-RandomOrganizations -Count
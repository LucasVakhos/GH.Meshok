using GH.Components;
namespace MeshokBrowser
{
    public class ConfigMain: AppConfig
    {
        public string RegCookie { get; set; }
        protected override void InitConfigs()
        {
            PrivateConfigs.Add(new ConfigBridge());
            PrivateConfigs.Add(new ConfigMeshok());
            PrivateConfigs.Add(new ConfigIShop());
        }
    }
}

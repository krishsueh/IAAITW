[assembly: Microsoft.Owin.OwinStartup(typeof(IAAITW.Startup))]

namespace IAAITW
{
    using System.Configuration;

    using CKSource.CKFinder.Connector.Config;
    using CKSource.CKFinder.Connector.Core.Builders;
    using CKSource.CKFinder.Connector.Core.Logs;
    using CKSource.CKFinder.Connector.Host.Owin;
    using CKSource.CKFinder.Connector.Logs.NLog;
    using CKSource.CKFinder.Connector.KeyValue.EntityFramework;
    using CKSource.FileSystem.Dropbox;
    using CKSource.FileSystem.Local;

    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    using Owin;
    using CKSource.CKFinder.Connector.Core.Acl;
    using CKSource.CKFinder.Connector.Core.Authentication;
    using System.Threading.Tasks;
    using System.Threading;
    using CKSource.CKFinder.Connector.Core;
    using System.Collections.Generic;

    public class Startup
    {
        public ConnectorBuilder ConfigureConnector()
        {
            var connectorBuilder = new ConnectorBuilder();
            connectorBuilder
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        config.AddProxyBackend("local", new LocalStorage(@"MyFiles"));
                        config.AddResourceType("Files", resourceBuilder => resourceBuilder.SetBackend("local", "files"));
                        config.AddAclRule(new AclRule(
                            new StringMatcher("*"), new StringMatcher("/"), new StringMatcher("*"),
                            new Dictionary<Permission, PermissionType>
                            {
                        { Permission.FolderView, PermissionType.Allow },
                        { Permission.FolderCreate, PermissionType.Allow },
                        { Permission.FolderRename, PermissionType.Allow },
                        { Permission.FolderDelete, PermissionType.Allow },

                        { Permission.FileView, PermissionType.Allow },
                        { Permission.FileCreate, PermissionType.Allow },
                        { Permission.FileRename, PermissionType.Allow },
                        { Permission.FileDelete, PermissionType.Allow },

                        { Permission.ImageResize, PermissionType.Allow },
                        { Permission.ImageResizeCustom, PermissionType.Allow }
                            }));
                    })
                .SetAuthenticator(new MyAuthenticator());

            return connectorBuilder;
        }
        public class MyAuthenticator : IAuthenticator
        {
            public Task<IUser> AuthenticateAsync(ICommandRequest commandRequest, CancellationToken cancellationToken)
            {
                var user = new User(true, null);
                return Task.FromResult((IUser)user);
            }
        }
        public void Configuration(IAppBuilder appBuilder)
        {
            var connectorBuilder = ConfigureConnector();
            var connector = connectorBuilder.Build(new OwinConnectorFactory());
            appBuilder.Map("/CKFinder/connector", builder => builder.UseConnector(connector));
        }
    }
}

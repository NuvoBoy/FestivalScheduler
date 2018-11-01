using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows.Backend{
    public class DriverManager{
        /// <summary>
        /// shared data
        /// </summary>
        private readonly ClientDataContext _dataContext;
        /// <summary>
        /// ctor
        /// </summary>
        public DriverManager(){
            _dataContext = ClientDataContext.GetInstance();
        }
       /// <summary>
        /// load all driver nodes
        /// </summary>
        public void LoadDriver(){
            try{
                var proxy = ServerConnector.GetInstance().GetDataProviderProxy();
                if(proxy == null || !_dataContext.ServerAvailable) return;

                var task = Task.Factory.StartNew(() => proxy.Invoke<IEnumerable<DriverNode>>("LoadDriverNodes").Result);
                task.Wait();
                var driver = task.Result;

                foreach (var node in driver){
                    _dataContext.Driver.GetOrAdd(node.NodeId, node);
                }

                _dataContext.UserChanged = true;
            }catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// create new driver node
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="permission"></param>
        /// <param name="membership"></param>
        /// <param name="phoneNumber"></param>
        public void CreateDriverNode(string username, string password, string name, PermissionGroups permission,
            DriverMembership membership, string phoneNumber){
            try{
                var proxy = ServerConnector.GetInstance().GetManagementProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                var task = Task.Factory.StartNew(() => proxy.Invoke<DriverNode>("CreateDriver", username, password, name, permission,
                    membership, phoneNumber).Result);
                task.Wait();
                var result = task.Result;
                if (result == null) return;

                _dataContext.Driver.GetOrAdd(result.NodeId, result);
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// delete driver node
        /// </summary>
        /// <param name="nodeId"></param>
        public void DeleteDriverNode(string nodeId){
            try{
                var proxy = ServerConnector.GetInstance().GetManagementProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                Task.Factory.StartNew(() => proxy.Invoke("DeleteNode", nodeId));
                _dataContext.Driver.TryRemove(nodeId, out _);
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// change existing driver node
        /// </summary>
        /// <param name="node"></param>
        public void ChangeDriverNode(DriverNode node){
            try{
                var proxy = ServerConnector.GetInstance().GetManagementProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                Task.Factory.StartNew(() => proxy.Invoke("ChangeNode", node));
                _dataContext.Driver[node.NodeId] = node;
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

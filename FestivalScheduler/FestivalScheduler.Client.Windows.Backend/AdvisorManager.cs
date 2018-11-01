using System;
using System.Collections.Generic;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows.Backend
{
    public class AdvisorManager
    {
        /// <summary>
        /// shared data
        /// </summary>
        private readonly ClientDataContext _dataContext;

        public AdvisorManager(){
            _dataContext = ClientDataContext.GetInstance();
        }
        /// <summary>
        /// load all advisor nodes
        /// </summary>
        public void LoadAdvisor(){
            try{
                var proxy = ServerConnector.GetInstance().GetDataProviderProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                //var advisor = _dataProviderProxy.Invoke<IEnumerable<AdvisorNode>>("LoadAdvisorNodes").Result;
                var advisor = proxy.Invoke<IEnumerable<AdvisorNode>>("LoadAdvisorNodes").Result;
                if (advisor == null) return;
                foreach (var node in advisor){
                    _dataContext.Advisor.GetOrAdd(node.NodeId, node);
                }
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// create new advisor node
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="permission"></param>
        /// <param name="choirId"></param>
        /// <param name="phoneNumber"></param>
        public void CreateAdvisorNode(string username, string password, string name, PermissionGroups permission,
            string choirId, string phoneNumber){
            try{
                //var result = _managementProxy.Invoke<AdvisorNode>("CreateAdvisor", username, password, name, permission,
                //    choirId, phoneNumber).Result;
                var proxy = ServerConnector.GetInstance().GetManagementProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                var result = proxy.Invoke<AdvisorNode>("CreateAdvisor", username, password, name, permission,
                    choirId, phoneNumber).Result;
                if (result == null) return;

                _dataContext.Advisor.GetOrAdd(result.NodeId, result);
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// delete entry
        /// </summary>
        /// <param name="nodeId"></param>
        public void DeleteAdvisorNode(string nodeId){
            try{
                var proxy = ServerConnector.GetInstance().GetManagementProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                proxy.Invoke("DeleteNode", nodeId);
                _dataContext.Advisor.TryRemove(nodeId, out _);
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// change existing entry
        /// </summary>
        /// <param name="node"></param>
        public void ChangeAdvisorNode(AdvisorNode node){
            try{
                var proxy = ServerConnector.GetInstance().GetManagementProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                proxy.Invoke("ChangeNode", node);
                _dataContext.Advisor[node.NodeId] = node;
                _dataContext.UserChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;
using Microsoft.AspNet.SignalR.Client;

namespace FestivalScheduler.Client.Windows.Backend
{
    public class ChoirManager
    {
        /// <summary>
        /// hub for user handling
        /// </summary>
        private IHubProxy _managementProxy;
        /// <summary>
        /// hub for data loading operations
        /// </summary>
        private IHubProxy _dataProviderProxy;
        /// <summary>
        /// config file
        /// </summary>
        private readonly BackendConfig _config;
        /// <summary>
        /// shared data
        /// </summary>
        private ClientDataContext _dataContext;
        /// <summary>
        /// ctor
        /// </summary>
        public ChoirManager()
        {
            _config = new BackendConfig();
            _dataContext = ClientDataContext.GetInstance();

            StartServerConnection();
            Thread.Sleep(500);
        }
        /// <summary>
        /// start server connection
        /// </summary>
        private async void StartServerConnection(){
            try{
                var hubConnection = new HubConnection(_config.ServerUrl);
                _managementProxy = hubConnection.CreateHubProxy(_config.ManagementHub);
                _dataProviderProxy = hubConnection.CreateHubProxy(_config.DataProviderHub);

                ServicePointManager.DefaultConnectionLimit = 10;
                await hubConnection.Start();
            }catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// load all choir nodes
        /// </summary>
        public void LoadChoirs(){
            try{
                var choirs = _dataProviderProxy.Invoke<IEnumerable<ChoirNode>>("LoadChoirNodes").Result;
                foreach (var node in choirs){
                    _dataContext.Choirs.GetOrAdd(node.NodeId, node);
                }

                _dataContext.ChoirsChanged = true;
            }catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// create new choir node
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shortCut"></param>
        /// <param name="nationality"></param>
        /// <param name="membercount"></param>
        /// <param name="accomodationPlace"></param>
        public void CreateChoirNode(string name, string shortCut, string nationality, int membercount = 0,
            string accomodationPlace = null){
            try{
                var result = _managementProxy.Invoke<ChoirNode>("CreateChoirNode", name, shortCut,
                    nationality, membercount, accomodationPlace).Result;
                if (result == null) return;

                _dataContext.Choirs.GetOrAdd(result.NodeId, result);
                _dataContext.ChoirsChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// delete choir node
        /// </summary>
        /// <param name="nodeId"></param>
        public void DeleteChoirNode(string nodeId){
            try{
                _managementProxy.Invoke("DeleteNode", nodeId);
                _dataContext.Choirs.TryRemove(nodeId, out _);
                _dataContext.ChoirsChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// delete choir node
        /// </summary>
        /// <param name="node"></param>
        public void ChangeChoirNode(ChoirNode node){
            try{
                _managementProxy.Invoke("ChangeNode", node);
                _dataContext.Choirs[node.NodeId] = node;
                _dataContext.ChoirsChanged = true;
            }
            catch (Exception e){
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

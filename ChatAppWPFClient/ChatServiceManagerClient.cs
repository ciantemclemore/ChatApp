﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatAppServiceLibrary.DataContracts
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Client", Namespace="http://schemas.datacontract.org/2004/07/ChatAppServiceLibrary.DataContracts")]
    public partial class Client : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.DateTime CreatedOnField;
        
        private System.Guid IdField;
        
        private string NameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime CreatedOn
        {
            get
            {
                return this.CreatedOnField;
            }
            set
            {
                if ((this.CreatedOnField.Equals(value) != true))
                {
                    this.CreatedOnField = value;
                    this.RaisePropertyChanged("CreatedOn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                if ((this.IdField.Equals(value) != true))
                {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NameField, value) != true))
                {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/ChatAppServiceLibrary.DataContracts")]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.Guid ChatRoomIdField;
        
        private string ChatRoomNameField;
        
        private string ContentField;
        
        private System.Guid IdField;
        
        private ChatAppServiceLibrary.DataContracts.Client ReceiverField;
        
        private ChatAppServiceLibrary.DataContracts.Client SenderField;
        
        private System.DateTime TimeStampField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ChatRoomId
        {
            get
            {
                return this.ChatRoomIdField;
            }
            set
            {
                if ((this.ChatRoomIdField.Equals(value) != true))
                {
                    this.ChatRoomIdField = value;
                    this.RaisePropertyChanged("ChatRoomId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ChatRoomName
        {
            get
            {
                return this.ChatRoomNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ChatRoomNameField, value) != true))
                {
                    this.ChatRoomNameField = value;
                    this.RaisePropertyChanged("ChatRoomName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Content
        {
            get
            {
                return this.ContentField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ContentField, value) != true))
                {
                    this.ContentField = value;
                    this.RaisePropertyChanged("Content");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                if ((this.IdField.Equals(value) != true))
                {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ChatAppServiceLibrary.DataContracts.Client Receiver
        {
            get
            {
                return this.ReceiverField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ReceiverField, value) != true))
                {
                    this.ReceiverField = value;
                    this.RaisePropertyChanged("Receiver");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ChatAppServiceLibrary.DataContracts.Client Sender
        {
            get
            {
                return this.SenderField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SenderField, value) != true))
                {
                    this.SenderField = value;
                    this.RaisePropertyChanged("Sender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime TimeStamp
        {
            get
            {
                return this.TimeStampField;
            }
            set
            {
                if ((this.TimeStampField.Equals(value) != true))
                {
                    this.TimeStampField = value;
                    this.RaisePropertyChanged("TimeStamp");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChatRoomRequest", Namespace="http://schemas.datacontract.org/2004/07/ChatAppServiceLibrary.DataContracts")]
    public partial class ChatRoomRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Client> ClientsField;
        
        private string DisplayNameField;
        
        private bool IsPublicField;
        
        private string ServerNameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Client> Clients
        {
            get
            {
                return this.ClientsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClientsField, value) != true))
                {
                    this.ClientsField = value;
                    this.RaisePropertyChanged("Clients");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DisplayName
        {
            get
            {
                return this.DisplayNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DisplayNameField, value) != true))
                {
                    this.DisplayNameField = value;
                    this.RaisePropertyChanged("DisplayName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsPublic
        {
            get
            {
                return this.IsPublicField;
            }
            set
            {
                if ((this.IsPublicField.Equals(value) != true))
                {
                    this.IsPublicField = value;
                    this.RaisePropertyChanged("IsPublic");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ServerName
        {
            get
            {
                return this.ServerNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ServerNameField, value) != true))
                {
                    this.ServerNameField = value;
                    this.RaisePropertyChanged("ServerName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChatRoom", Namespace="http://schemas.datacontract.org/2004/07/ChatAppServiceLibrary.DataContracts")]
    public partial class ChatRoom : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Client> ClientsField;
        
        private string DisplayNameField;
        
        private System.Guid IdField;
        
        private bool IsPublicField;
        
        private string LastMessageField;
        
        private System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Message> MessagesField;
        
        private string ReceiverTitleField;
        
        private string SenderTitleField;
        
        private string ServerNameField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Client> Clients
        {
            get
            {
                return this.ClientsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ClientsField, value) != true))
                {
                    this.ClientsField = value;
                    this.RaisePropertyChanged("Clients");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DisplayName
        {
            get
            {
                return this.DisplayNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DisplayNameField, value) != true))
                {
                    this.DisplayNameField = value;
                    this.RaisePropertyChanged("DisplayName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                if ((this.IdField.Equals(value) != true))
                {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsPublic
        {
            get
            {
                return this.IsPublicField;
            }
            set
            {
                if ((this.IsPublicField.Equals(value) != true))
                {
                    this.IsPublicField = value;
                    this.RaisePropertyChanged("IsPublic");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastMessage
        {
            get
            {
                return this.LastMessageField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LastMessageField, value) != true))
                {
                    this.LastMessageField = value;
                    this.RaisePropertyChanged("LastMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Message> Messages
        {
            get
            {
                return this.MessagesField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MessagesField, value) != true))
                {
                    this.MessagesField = value;
                    this.RaisePropertyChanged("Messages");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReceiverTitle
        {
            get
            {
                return this.ReceiverTitleField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ReceiverTitleField, value) != true))
                {
                    this.ReceiverTitleField = value;
                    this.RaisePropertyChanged("ReceiverTitle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SenderTitle
        {
            get
            {
                return this.SenderTitleField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SenderTitleField, value) != true))
                {
                    this.SenderTitleField = value;
                    this.RaisePropertyChanged("SenderTitle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ServerName
        {
            get
            {
                return this.ServerNameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ServerNameField, value) != true))
                {
                    this.ServerNameField = value;
                    this.RaisePropertyChanged("ServerName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IChatManagerService", CallbackContract=typeof(IChatManagerServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
public interface IChatManagerService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatManagerService/Login", ReplyAction="http://tempuri.org/IChatManagerService/LoginResponse")]
    bool Login(ChatAppServiceLibrary.DataContracts.Client client);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatManagerService/Login", ReplyAction="http://tempuri.org/IChatManagerService/LoginResponse")]
    System.Threading.Tasks.Task<bool> LoginAsync(ChatAppServiceLibrary.DataContracts.Client client);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/SendMessage")]
    void SendMessage(ChatAppServiceLibrary.DataContracts.Message message);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/SendMessage")]
    System.Threading.Tasks.Task SendMessageAsync(ChatAppServiceLibrary.DataContracts.Message message);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatManagerService/CreatePrivateChatRoom", ReplyAction="http://tempuri.org/IChatManagerService/CreatePrivateChatRoomResponse")]
    ChatAppServiceLibrary.DataContracts.ChatRoom CreatePrivateChatRoom(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatManagerService/CreatePrivateChatRoom", ReplyAction="http://tempuri.org/IChatManagerService/CreatePrivateChatRoomResponse")]
    System.Threading.Tasks.Task<ChatAppServiceLibrary.DataContracts.ChatRoom> CreatePrivateChatRoomAsync(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatManagerService/CreatePublicChatRoom", ReplyAction="http://tempuri.org/IChatManagerService/CreatePublicChatRoomResponse")]
    bool CreatePublicChatRoom(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatManagerService/CreatePublicChatRoom", ReplyAction="http://tempuri.org/IChatManagerService/CreatePublicChatRoomResponse")]
    System.Threading.Tasks.Task<bool> CreatePublicChatRoomAsync(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/JoinChatRoom")]
    void JoinChatRoom(System.Guid chatRoomId, ChatAppServiceLibrary.DataContracts.Client client);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/JoinChatRoom")]
    System.Threading.Tasks.Task JoinChatRoomAsync(System.Guid chatRoomId, ChatAppServiceLibrary.DataContracts.Client client);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, IsTerminating=true, Action="http://tempuri.org/IChatManagerService/Logout")]
    void Logout(string userName);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, IsTerminating=true, Action="http://tempuri.org/IChatManagerService/Logout")]
    System.Threading.Tasks.Task LogoutAsync(string userName);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IChatManagerServiceCallback
{
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/ReceiveMessage")]
    void ReceiveMessage(ChatAppServiceLibrary.DataContracts.Message message);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/UpdateOnlineClients")]
    void UpdateOnlineClients(System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.Client> clients);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatManagerService/UpdatePublicChatRooms")]
    void UpdatePublicChatRooms(System.Collections.Generic.List<ChatAppServiceLibrary.DataContracts.ChatRoom> chatRooms);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IChatManagerServiceChannel : IChatManagerService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class ChatManagerServiceClient : System.ServiceModel.DuplexClientBase<IChatManagerService>, IChatManagerService
{
    
    public ChatManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
            base(callbackInstance)
    {
    }
    
    public ChatManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
            base(callbackInstance, endpointConfigurationName)
    {
    }
    
    public ChatManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }
    
    public ChatManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress)
    {
    }
    
    public ChatManagerServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, binding, remoteAddress)
    {
    }
    
    public bool Login(ChatAppServiceLibrary.DataContracts.Client client)
    {
        return base.Channel.Login(client);
    }
    
    public System.Threading.Tasks.Task<bool> LoginAsync(ChatAppServiceLibrary.DataContracts.Client client)
    {
        return base.Channel.LoginAsync(client);
    }
    
    public void SendMessage(ChatAppServiceLibrary.DataContracts.Message message)
    {
        base.Channel.SendMessage(message);
    }
    
    public System.Threading.Tasks.Task SendMessageAsync(ChatAppServiceLibrary.DataContracts.Message message)
    {
        return base.Channel.SendMessageAsync(message);
    }
    
    public ChatAppServiceLibrary.DataContracts.ChatRoom CreatePrivateChatRoom(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest)
    {
        return base.Channel.CreatePrivateChatRoom(chatRoomRequest);
    }
    
    public System.Threading.Tasks.Task<ChatAppServiceLibrary.DataContracts.ChatRoom> CreatePrivateChatRoomAsync(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest)
    {
        return base.Channel.CreatePrivateChatRoomAsync(chatRoomRequest);
    }
    
    public bool CreatePublicChatRoom(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest)
    {
        return base.Channel.CreatePublicChatRoom(chatRoomRequest);
    }
    
    public System.Threading.Tasks.Task<bool> CreatePublicChatRoomAsync(ChatAppServiceLibrary.DataContracts.ChatRoomRequest chatRoomRequest)
    {
        return base.Channel.CreatePublicChatRoomAsync(chatRoomRequest);
    }
    
    public void JoinChatRoom(System.Guid chatRoomId, ChatAppServiceLibrary.DataContracts.Client client)
    {
        base.Channel.JoinChatRoom(chatRoomId, client);
    }
    
    public System.Threading.Tasks.Task JoinChatRoomAsync(System.Guid chatRoomId, ChatAppServiceLibrary.DataContracts.Client client)
    {
        return base.Channel.JoinChatRoomAsync(chatRoomId, client);
    }
    
    public void Logout(string userName)
    {
        base.Channel.Logout(userName);
    }
    
    public System.Threading.Tasks.Task LogoutAsync(string userName)
    {
        return base.Channel.LogoutAsync(userName);
    }
}

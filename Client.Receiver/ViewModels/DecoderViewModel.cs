namespace Client.Receiver
{
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Client.Receiver.Commands;
    using LubyTransform.Decode;
    using Entities;
    using Client.Receiver.EncodeService;

    public class DecoderViewModel : ViewModelBase
    {
        #region Member Variables

        const int overHead = 20;
        string _decodedMessage;
        string _statusMessage;
        readonly IDecode _decoder;
        readonly Client.Receiver.EncodeService.IEncodeService encodeServiceClient;

        #endregion

        #region Constructor

        public DecoderViewModel(IDecode decoder, IEncodeService encodeServiceClient)
        {
            this._decoder = decoder;
            this.encodeServiceClient = encodeServiceClient;
        } 

        #endregion

        #region Public Properties

        public string DecodedMessage
        {
            get { return _decodedMessage; }
            set
            {
                if (_decodedMessage != value)
                {
                    _decodedMessage = value;
                    OnPropertyChanged("DecodedMessage");
                }
            }
        }

        public string Status
        {
            get { return _statusMessage; }
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        #endregion

        #region Commands

        public ICommand StartReceiving
        {
            get
            {
                return new RelayCommand(StartReceivingExecute, CanStartReceivingExecute);
            }
        }

        #endregion

        #region Command Methods

        void StartReceivingExecute()
        {
            Status = "Receiving Message ";
            DecodedMessage = ReceiveMessage();
            Status = "Message Received";
        }

        bool CanStartReceivingExecute()
        {
            return true;
        }

        #endregion

        #region Private Methods

        private string ReceiveMessage()
        {
            var blocksCount = encodeServiceClient.GetNumberOfBlocks();
            var fileSize = encodeServiceClient.GetFileSize();
            var chunkSize = encodeServiceClient.GetChunkSize();
            IList<Drop> goblet = new List<Drop>();

            for (int i = 0; i < blocksCount + overHead; i++)
            {
                var drop = encodeServiceClient.Encode();
                goblet.Add(drop);
            }

            var fileData = _decoder.Decode(goblet, blocksCount, chunkSize, fileSize);
            return Encoding.ASCII.GetString(fileData);
        }

        #endregion
    }
}
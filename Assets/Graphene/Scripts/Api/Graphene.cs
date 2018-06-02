using Ditch.BitShares;
using Ditch.BitShares.Models.Operations;
using Ditch.Core;
using UnityEngine;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using System.Threading;
using Ditch.BitShares.Models;
using Cryptography.ECDSA;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Nano.Blockchain {
	public class Graphene : MonoBehaviour {
        public InputField inputAccountName;

        public Toggle toggle1, toggle2, toggle3;

        public Button buttonAccountCreate;

        public Text textStatusAccountName;

        public List<string> errors;

        public GameObject panelInfo;

        private bool isAccountNameCorrect = false;

        private OperationManager _operationManager;

        public void Init()
        {
            //specify how to parse json
            var jss = new JsonSerializerSettings
            {
                //DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK",
                Culture = CultureInfo.InvariantCulture
            };

            //specify how to connect to blockchain
            //var cm = new WebSocketManager(jss, 10000, 1000);
            var cm = new HttpManager(jss);
            _operationManager = new OperationManager(cm, jss);

            ConnectToNode();

            CheckAccountNameStatus();
        }

        void Awake()
        {
            Init();
        }

        public void ConnectToNode()
        {
            //var cUrls = new List<string> { "wss://dex.rnglab.org" };
            //var cUrls = new List<string> { "wss://node.testnet.bitshares.eu" };
            var cUrls = new List<string> { "https://node.testnet.bitshares.eu" };
            var conectedTo = _operationManager.TryConnectTo(cUrls, CancellationToken.None);

            Debug.Log(conectedTo);
        }

        public void AccountCreate()
        {
            buttonAccountCreate.interactable = false;

            IList<byte[]> YouPrivateKeys = new List<byte[]>
            {
                Ditch.Core.Base58.DecodePrivateWif("5JMCfREHnK4x5tPjruhEHJKX6wQDVBJFzikc1wxEBQ4iFTcHgMY")
            };

            string NewUserName = inputAccountName.text;

            //var privateKey = Secp256K1Manager.GenerateRandomKey();

            //var privateWif = Ditch.Core.Base58.EncodePrivateWif(privateKey);

            //var subWif = Ditch.Core.Base58.GetSubWif(NewUserName, privateWif, "active");
            //var pk = Ditch.Core.Base58.DecodePrivateWif(subWif);
            //var subPublicKey = Secp256K1Manager.GetPublicKey(pk, true);

            //subWif = Ditch.Core.Base58.GetSubWif(NewUserName, privateWif, "owner");
            //pk = Ditch.Core.Base58.DecodePrivateWif(subWif);
            //var subPublicKey2 = Secp256K1Manager.GetPublicKey(pk, true);

            var accountCreateOp = new AccountCreateOperation
            {
                Fee = new Asset()
                {
                    Amount = 0,
                    AssetId = new AssetIdType(1, 3, 0)
                },
                Registrar = new ObjectId(1, 2, 22765),
                Referrer = new ObjectId(1, 2, 22765),
                ReferrerPercent = 7000,
                Name = NewUserName,
                Owner = new Authority(new PublicKeyType("TEST6FcnaaaP7eZWqUs5VHRPMwjD69V1SFAAVoJRHiq3YEJ7tVgyQc", "TEST")),
                Active = new Authority(new PublicKeyType("TEST6KCYpvc3syCbsRaofXLffEXoLDKixN2SLUCLPz4XubmptGNFfe", "TEST")),
                Options = new AccountOptions()
                {
                    MemoKey = new PublicKeyType("TEST6HWVwXazrgS3MsWZvvSV6qdRbc8GS7KpdfDw8mAcNug4RcPv3v", "TEST"),
                    VotingAccount = new AccountIdType(1, 2, 5),
                },
            };
            
            var responce = _operationManager.BroadcastOperations(YouPrivateKeys, CancellationToken.None, accountCreateOp);

            if (false == responce.IsError)
            {
                panelInfo.SetActive(true);
            }

            //Debug.Log("RESULT = " + responce.Result);
            //Debug.Log("ERROR = " + responce.GetErrorMessage());
        }

        private void Update()
        {
            if (toggle1.isOn && toggle2.isOn && toggle3.isOn && isAccountNameCorrect)
            {
                buttonAccountCreate.interactable = true;
            } else
            {
                buttonAccountCreate.interactable = false;
            }
        }

        public void CheckAccountNameStatus()
        {
            inputAccountName.text = CheckAllowedLetters(inputAccountName.text);

            if (string.IsNullOrEmpty(inputAccountName.text))
            {
                isAccountNameCorrect = false;
                textStatusAccountName.color = Color.red;
                textStatusAccountName.text = errors[6];
            }
            else if (false == Regex.IsMatch(inputAccountName.text, "^[a-zA-Z]"))
            {
                isAccountNameCorrect = false;
                textStatusAccountName.color = Color.red;
                textStatusAccountName.text = errors[0];
            }
            else if (inputAccountName.text.Length < 3)
            {
                isAccountNameCorrect = false;
                textStatusAccountName.color = Color.red;
                textStatusAccountName.text = errors[1];
            }
            else if (false == Regex.IsMatch(inputAccountName.text, "[\\-\\d]"))
            {
                isAccountNameCorrect = false;
                textStatusAccountName.color = Color.yellow;
                textStatusAccountName.text = errors[2];
            }
            else if (inputAccountName.text.Contains("--"))
            {
                isAccountNameCorrect = false;
                textStatusAccountName.color = Color.red;
                textStatusAccountName.text = errors[3];
            }
            else if (false == Regex.IsMatch(inputAccountName.text, "[a-zA-Z\\d]$"))
            {
                isAccountNameCorrect = false;
                textStatusAccountName.color = Color.red;
                textStatusAccountName.text = errors[4];
            }
            else
            {
                isAccountNameCorrect = true;
                textStatusAccountName.color = Color.green;
                textStatusAccountName.text = errors[7];
            }
        }

        private string CheckAllowedLetters(string s)
        {
            string rez = string.Empty;

            for (int i=0; i < s.Length; i++)
            {
                if (Regex.IsMatch(s[i].ToString(), "[a-zA-Z\\d\\-]"))
                {
                    rez += s[i].ToString();
                }
            }

            return rez;
        }
    }
}
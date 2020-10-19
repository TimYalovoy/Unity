using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TimurYalovoy.UserWallet
{
    public class CurrencyDisplay : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected Wallet wallet;
        [SerializeField] Font font;

        protected Vector2 textField_size;
        protected Vector2 buttonField_size;
        protected Vector2 bttnTextField_size;
        private GameObject currencyDisplay;
        private GameObject wallet_go;
        private GameObject open_go;
        private GameObject close_go;
        #endregion

        void Start()
        {
            textField_size = new Vector2(50, 15);
            buttonField_size = new Vector2(20, 20);
            bttnTextField_size = new Vector2(35, 30);

            GameObject walletDisplay;
            Canvas myCanvas;

            #region Wallet
            GameObject canvas = new GameObject { name = "Canvas" };
            canvas.AddComponent<Canvas>();
            walletDisplay = new GameObject { name = "Wallet" };
            walletDisplay.transform.SetParent(canvas.transform);
            walletDisplay.AddComponent<RectTransform>();
            walletDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(1024, 768);

            myCanvas = canvas.GetComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            myCanvas.worldCamera = FindObjectOfType<Camera>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            walletDisplay.AddComponent<GridLayoutGroup>();
            #endregion

            #region EventSystem
            GameObject eventSystem = new GameObject { name = "EventSystem" };
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            #endregion

            #region Open \ Close Buttons
            //walletDisplay.SetActive(false);
            CreateButtonWithText("Open", canvas, null);
            CreateButtonWithText("Close", canvas, null);
            #endregion

            #region Grid of currencies
            GridLayoutGroup grid = walletDisplay.GetComponent<GridLayoutGroup>();
            grid.padding.left = 170;
            grid.padding.top = 100;
            grid.spacing = new Vector2(350, 250);
            #endregion

            foreach (Currency currency in wallet.currencies)
            {

                #region Currency
                /*
                 * Currency
                 *
                */
                currencyDisplay = new GameObject { name = currency.CName };
                currencyDisplay.transform.SetParent(walletDisplay.transform);

                RectTransform cd_rectTransform = currencyDisplay.AddComponent<RectTransform>();
                cd_rectTransform.sizeDelta = new Vector2(45, 45);
                cd_rectTransform.localPosition = new Vector3(0, 0, 0);

                // STRETCH
                cd_rectTransform.anchorMax.Set(1f, 1f);
                cd_rectTransform.anchorMin.Set(0f, 0f);
                #endregion

                /*
                 * CURRENCY PROPETIES
                 *
                 */
                #region Value - currency property
                GameObject cd_go_Value = CreateTextField("Value", currencyDisplay, currency.Value.ToString());
                // buttons
                // CreateButtonWithText("Name method of currency", parent_GameObject, currency.IOperableMethods())
                CreateButtonWithText("Increment", cd_go_Value, currency);
                CreateButtonWithText("Decrement", cd_go_Value, currency);
                CreateButtonWithText("Zeroing", cd_go_Value, currency);
                #endregion

                #region Name - currency property
                _ = CreateTextField("Name", currencyDisplay, currency.CName);
                #endregion

                #region Sprite - currency property
                /* 
                 * Sprite - IMAGE
                 * 
                 */
                GameObject cd_go_Sprite = new GameObject { name = "Sprite" };
                cd_go_Sprite.transform.SetParent(currencyDisplay.transform);
                // **
                Image currencySprite = cd_go_Sprite.AddComponent<Image>();
                currencySprite.sprite = currency.Sprite;
                // **
                RectTransform cS_rectTransform = cd_go_Sprite.GetComponent<RectTransform>();
                cS_rectTransform.sizeDelta = new Vector2(30, 30);
                cS_rectTransform.localPosition = new Vector3(0, 20);
                #endregion

            }
            wallet_go = GameObject.Find("Wallet");
            open_go = GameObject.Find("Open");
            close_go = GameObject.Find("Close");
            open_go.SetActive(false);
        }

        #region Create Text Field Method
        private GameObject CreateTextField(string PropertieName, GameObject parentField, string val)
        {
            GameObject currencyPropertie = new GameObject { name = PropertieName };
            currencyPropertie.transform.SetParent(parentField.transform);

            Text currencyPropertieText = currencyPropertie.AddComponent<Text>();
            currencyPropertieText.font = font;
            currencyPropertieText.fontSize = 12;
            currencyPropertieText.alignment = TextAnchor.MiddleCenter;
            currencyPropertieText.text = val;

            RectTransform cpText_rectTransform = currencyPropertie.GetComponent<RectTransform>();
            cpText_rectTransform.sizeDelta = textField_size;
            if (PropertieName == "Name")
            {
                cpText_rectTransform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                cpText_rectTransform.localPosition = new Vector3(0, -20, 0);
            }

            return currencyPropertie;
        }
        #endregion

        #region Create Button with Text Component Method
        private void CreateButtonWithText(string bttn_name, GameObject parentField, Currency currency)
        {
            GameObject Button_go = new GameObject { name = bttn_name };
            Button_go.transform.SetParent(parentField.transform);

            Button button = Button_go.AddComponent<Button>();
            Button_go.AddComponent<RectTransform>();
            button.transform.SetParent(Button_go.transform);

            Button_go.AddComponent<Image>();
            Button_go.GetComponent<Image>().sprite = Resources.Load("Background") as Sprite;
            Button_go.GetComponent<Image>().color = Color.grey;

            GameObject buttonText_go;

            RectTransform button_rectTransform = button.GetComponent<RectTransform>();
            button_rectTransform.sizeDelta = buttonField_size;
            switch (bttn_name)
            {
                case "Increment":
                    {
                        button.onClick.AddListener(delegate { currency.Increment(); });
                        button.GetComponent<RectTransform>().localPosition = new Vector3(45, 0, 0);
                        buttonText_go = new GameObject { name = "+" };
                        break;
                    }
                case "Decrement":
                    {
                        button.onClick.AddListener(delegate { currency.Decrement(); });
                        button.GetComponent<RectTransform>().localPosition = new Vector3(-45, 0, 0);
                        buttonText_go = new GameObject { name = "-" };
                        break;
                    }
                case "Zeroing":
                    {
                        button.onClick.AddListener(delegate { currency.ZeroingValue(); });
                        button.GetComponent<RectTransform>().localPosition = new Vector3(0, -20, 0);
                        buttonText_go = new GameObject { name = "Zeroing" };
                        button_rectTransform.sizeDelta = new Vector2(45, 20);
                        break;
                    }
                case "Open":
                    {
                        buttonText_go = new GameObject { name = "Open" };
                        button.onClick.AddListener(delegate { this.Open(); });
                        button_rectTransform.localPosition = new Vector3(0, 0, 0);
                        button_rectTransform.sizeDelta = new Vector2(45, 20);
                        break;
                    }
                case "Close":
                    {
                        buttonText_go = new GameObject { name = "Close" };
                        button.onClick.AddListener(delegate { this.Close(); });
                        button_rectTransform.localPosition = new Vector3(425, 345, 0);
                        button_rectTransform.sizeDelta = new Vector2(45, 20);
                        break;
                    }
                default:
                    {
                        Debug.Log("Wrong name. Button name must contain the name one of the Currency methods:/n Increment, Decrement or Zeroing");
                        buttonText_go = new GameObject { name = "Error" };
                        break;
                    }
            }

            buttonText_go.transform.SetParent(Button_go.transform);

            Text button_Text = buttonText_go.AddComponent<Text>();
            button_Text.font = font;
            button_Text.fontSize = 9;
            button_Text.text = buttonText_go.name;
            button_Text.alignment = TextAnchor.MiddleCenter;

            RectTransform button_Text_rectTransform = button_Text.GetComponent<RectTransform>();
            button_Text_rectTransform.sizeDelta = bttnTextField_size;
            button_Text_rectTransform.localPosition = new Vector3(0, 0, 0);
        }
        #endregion

        #region Open / Close Methods
        private void Open()
        {
            wallet_go.SetActive(true);
            close_go.SetActive(true);
            open_go.SetActive(false);
            wallet.UW_Load();
        }

        private void Close()
        {
            wallet_go.SetActive(false);
            close_go.SetActive(false);
            open_go.SetActive(true);
            wallet.UW_Save();
        }
        #endregion

        private void Update()
        {
            if (wallet_go.activeInHierarchy)
            {
                Text[] texts = new Text[wallet.currencies.Count];
                foreach (Currency currency in wallet.currencies)
                {
                    for (int i = 0; i < texts.Length; i++)
                    {
                        texts[i] = GameObject.Find(currency.CName).GetComponentInChildren<Text>();
                        texts[i].text = currency.Value.ToString();
                    }
                }
            }
        }
    }
}

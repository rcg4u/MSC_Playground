using UnityEngine;

namespace FleetariTowingService
{
    public class GuiClass
    {
        private readonly FleetariTowingService _fleetariTowingService;

        public GuiClass(FleetariTowingService fleetariTowingService)
        {
            _fleetariTowingService = fleetariTowingService;
        }

        //Gui styling
        internal static void GuiStyling(out GUIStyle textStyle, out GUIStyle serviceStyle, out GUIStyle noMoneyStyle,
            out GUIStyle modalStyle)
        {
            textStyle = GUI.skin.GetStyle("label");
            serviceStyle = new GUIStyle();
            noMoneyStyle = new GUIStyle();
            modalStyle = new GUIStyle();
            textStyle.alignment = TextAnchor.MiddleCenter;

            noMoneyStyle.fontSize = 30;
            noMoneyStyle.alignment = TextAnchor.UpperCenter;

            serviceStyle.fontSize = 20;
            serviceStyle.fontStyle = FontStyle.Bold;
            serviceStyle.normal.textColor = Color.white;
            serviceStyle.alignment = TextAnchor.MiddleCenter;
            serviceStyle.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        //Show hand texture
        internal void ShowUseGui(GUIStyle textStyle, GUIStyle serviceStyle)
        {
            if (_fleetariTowingService.ShowGui)
            {
                GUI.Label(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50),
                    _fleetariTowingService.HandUseTexture, textStyle);
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 15, 200, 50), "TOWING SERVICE",
                    serviceStyle);
            }
        }

        //Show tow paper
        internal void ShowPaper(GUIStyle modalStyle)
        {
            if (_fleetariTowingService.ShowTowMenu)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 350, 500, 700),
                    Texture2D.whiteTexture);
                GUI.ModalWindow(10, new Rect(Screen.width / 2 - 250, Screen.height / 2 - 350, 500, 700),
                    GuiSettingsWindow, "", modalStyle);

                if (!_fleetariTowingService.ShowMouse)
                {
                    PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerInMenu").Value = true;
                    _fleetariTowingService.ShowMouse = true;
                }
            }
        }

        //Show not enough money
        internal void ShowNotEnoughMoney(GUIStyle noMoneyStyle)
        {
            if (_fleetariTowingService.ShowNoMoneyLabel)
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 20, 200, 40),
                    "<color=orange><b>Not Enough Money</b></color>", noMoneyStyle);
        }

        //Show tow in progress
        internal void ShowTowInProgress(GUIStyle noMoneyStyle)
        {
            if (_fleetariTowingService.ShowTowInProgressLabel)
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 20, 200, 40),
                    "<color=orange><b>Tow already in progress. Come back later.</b></color>", noMoneyStyle);
        }

        //Show all buttons and labels on the paper
        internal void GuiSettingsWindow(int id)
        {
            var labelPricestyle = new GUIStyle
            {
                fontSize = 14,
                alignment = TextAnchor.UpperRight              
            };
            var buttonStyle = new GUIStyle
            {
                fontSize = 14,
                normal = {textColor = Color.black},
                hover = {textColor = Color.blue},
                onHover = {textColor = Color.blue},
                active = {textColor = Color.blue},
                onFocused = {textColor = Color.blue},
                alignment = TextAnchor.UpperLeft
            };



            //Show title and extra labels
            ExtraLabels();

            //Show extra buttons
            ExtraButtons();

            //Checkbox Buttons
            SatsumaCheckbox(labelPricestyle, buttonStyle);
            FerndaleCheckbox(labelPricestyle, buttonStyle);
            HayosikoCheckbox(labelPricestyle, buttonStyle);
            GifuCheckbox(labelPricestyle, buttonStyle);
            KekmetCheckbox(labelPricestyle, buttonStyle);
            FlatbedCheckbox(labelPricestyle, buttonStyle);
            JonnezCheckbox(labelPricestyle, buttonStyle);
        }

        //Title and extra labels
        private void ExtraLabels()
        {
            var labelstyle = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                normal = {textColor = Color.black},
                fontSize = 20
            };


            GUI.Label(new Rect(20, 10, 500, 80),
                "Fleetarin Hinauspalvelu\nFleetaris Bärgningstjänst\nFleetari's Towing Service", labelstyle);
            GUI.Label(new Rect(20, 10, 500, 80),
                "Fleetarin Hinauspalvelu\nFleetaris Bärgningstjänst\nFleetari's Towing Service", labelstyle);

            GUI.Label(new Rect(20, 630, 300, 30),
                "<size=14>Ajoneuvosi hinataan " + _fleetariTowingService.TowDestinationFinnish + "</size>", labelstyle);
            GUI.Label(new Rect(20, 650, 300, 30),
                "<size=14>Ditt fordon kommer levereras till " + _fleetariTowingService.TowDestinationSwedish +
                "</size>", labelstyle);
            GUI.Label(new Rect(20, 670, 300, 30),
                "<size=14>Your vehicle will be delivered to " + _fleetariTowingService.TowDestination + "</size>",
                labelstyle);
        }   

        //Order button and extra buttons
        private void ExtraButtons()
        {
            var orderButtonStyle = new GUIStyle
            {
                fontSize = 20,
                normal = {textColor = Color.black},
                alignment = TextAnchor.UpperLeft
            };
            var itemButtonStyle = new GUIStyle
            {
                alignment = TextAnchor.LowerLeft,
                fontSize = 14
            };
            var itemLabelStyle = new GUIStyle
            {
                fontSize = 14,
                alignment = TextAnchor.UpperLeft
            };
            var extraCheckButtonStyle = new GUIStyle
            {
                alignment = TextAnchor.LowerRight,
                fontSize = 14
            };
            var extraCheckLabelStyle = new GUIStyle
            {
                alignment = TextAnchor.UpperRight,
                fontSize = 14
            };
            var closeButton = new GUIStyle
            {
                alignment = TextAnchor.MiddleRight
            };


            //Checkbox itemdelivery
            GUI.Label(new Rect(20, 525, 230, 20),
                "Toimita esineet lähistöltä.", itemLabelStyle);
            GUI.Label(new Rect(20, 542, 230, 20),
                "Frakt av närliggande objekt.", itemLabelStyle);
            GUI.Label(new Rect(20, 559, 230, 20),
                "Delivery of nearby items.", itemLabelStyle);
            if (GUI.Button(new Rect(20, 530, 230, 75),          
                "Hinta, Pris, Price: <b>500,-</b>   " + "<size=25>" + _fleetariTowingService.CheckBoxItems + " </size>", itemButtonStyle))
                if (_fleetariTowingService.CheckBoxItems == "☐")
                {
                    _fleetariTowingService.CheckBoxItems = "☑";
                    _fleetariTowingService.NearByItemsSelected = true;
                    _fleetariTowingService.Total += 500;
                }
                else
                {
                    _fleetariTowingService.CheckBoxItems = "☐";
                    _fleetariTowingService.NearByItemsSelected = false;
                    _fleetariTowingService.Total -= 500;
                }


            //Satsuma Extra Checkbox
            if (_fleetariTowingService.CheckBoxSatsumaExtraEnabled) { 
                GUI.Label(new Rect(250, 525, 230, 20),
                    "Satsuman toimitus korjaamolle.", extraCheckLabelStyle);
            GUI.Label(new Rect(250, 542, 230, 20),
                "Bogsera Satsuman till verkstaden.", extraCheckLabelStyle);
            GUI.Label(new Rect(250, 559, 230, 20),
                "Tow Satsuma to the repairshop.", extraCheckLabelStyle);
            if (GUI.Button(new Rect(250, 530, 230, 75),
                    "\nHinta, Pris, Price: <b>2000,-</b>   " + "<size=25>" + _fleetariTowingService.CheckBoxSatsumaExtra + " </size>", extraCheckButtonStyle))
                    if (_fleetariTowingService.CheckBoxSatsumaExtra == "☑")
                    {
                        _fleetariTowingService.CheckBoxSatsumaExtra = "☐";
                        _fleetariTowingService.TowDestination = "your home.";
                        _fleetariTowingService.TowDestinationSwedish = "ditt hem.";
                        _fleetariTowingService.TowDestinationFinnish = "kotiin.";
                        _fleetariTowingService.Total -= 2000;
                    }
                    else
                    {
                        _fleetariTowingService.CheckBoxSatsumaExtra = "☑";
                        _fleetariTowingService.TowDestination = "the repairshop.";
                        _fleetariTowingService.TowDestinationSwedish = "verkstaden.";
                        _fleetariTowingService.TowDestinationFinnish = "korjaamolle.";
                        _fleetariTowingService.Total += 2000;
                    }
            }

            //Orderbutton
            if (GUI.Button(new Rect(390, 620, 100, FleetariTowingService.ButtonHeight), "<b>TILAA\nBESTÄLL\nORDER</b>",
                orderButtonStyle))
                _fleetariTowingService.CheckSelectedVehicle();

            GUI.Label(new Rect(330, 650, 100, 30),
                "Total:\n<b>" + _fleetariTowingService.Total + "</b>", itemLabelStyle);

            //Close button
            if (GUI.Button(new Rect(430, 20, 50, 50), "<size=30>✘</size>", closeButton))
                _fleetariTowingService.HidePaper();
        }

        private void JonnezCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 460, 230, 30),
                "Hinta, Pris, Price: <b>2000,-</b>\n<b>Jonnez ES <size=30>" + _fleetariTowingService.CheckBoxMoped +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 460, FleetariTowingService.ButtonWidth, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxMoped = "☑";
                _fleetariTowingService.Total += 2000;
            }
        }

        private void FlatbedCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 400, 230, 30),
                "Hinta, Pris, Price: <b>2000,-</b>\n<b>Flatbed <size=30>" + _fleetariTowingService.CheckBoxFlatbed +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 400, FleetariTowingService.ButtonWidth, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxFlatbed = "☑";
                _fleetariTowingService.Total += 2000;
            }
        }

        private void KekmetCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 340, 230, 30),
                "Hinta, Pris, Price: <b>6000,-</b>\n<b>Kekmet <size=30>" + _fleetariTowingService.CheckBoxTractor +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 340, FleetariTowingService.ButtonWidth, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxTractor = "☑";
                _fleetariTowingService.Total += 6000;
            }
        }

        private void GifuCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 280, 230, FleetariTowingService.ButtonWidth),
                "Hinta, Pris, Price: <b>10000,-</b>\n<b>Gifu <size=30>" + _fleetariTowingService.CheckBoxTruck +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 280, 500, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxTruck = "☑";
                _fleetariTowingService.Total += 10000;
            }
        }

        private void HayosikoCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 220, 230, 30),
                "Hinta, Pris, Price: <b>5000,-</b>\n<b>Hayosiko <size=30>" + _fleetariTowingService.CheckBoxVan +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 220, FleetariTowingService.ButtonWidth, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxVan = "☑";
                _fleetariTowingService.Total += 5000;
            }
        }

        private void FerndaleCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 160, 230, 30),
                "Hinta, Pris, Price: <b>4000,-</b>\n<b>Ferndale <size=30>" + _fleetariTowingService.CheckBoxFerndale +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 160, FleetariTowingService.ButtonWidth, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesFerndaleString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxFerndale = "☑";
                _fleetariTowingService.TowDestination = "the repairshop.";
                _fleetariTowingService.TowDestinationSwedish = "verkstaden.";
                _fleetariTowingService.TowDestinationFinnish = "korjaamolle.";
                _fleetariTowingService.Total += 4000;
            }
        }

        private void SatsumaCheckbox(GUIStyle labelPricestyle, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(250, 100, 230, 30),
                "Hinta, Pris, Price: <b>3000,-</b>\n<b>Satsuma AMP <size=30>" + _fleetariTowingService.CheckBoxSatsuma +
                "</size>   </b>", labelPricestyle);
            if (GUI.Button(new Rect(20, 100, FleetariTowingService.ButtonWidth, FleetariTowingService.ButtonHeight),
                FleetariTowingService.TowingLanguagesString, buttonStyle))
            {
                _fleetariTowingService.UnCheckAll();
                _fleetariTowingService.CheckBoxSatsuma = "☑";
                _fleetariTowingService.CheckBoxSatsumaExtraEnabled = true;
                _fleetariTowingService.Total += 3000;
            }
        }
    }
}
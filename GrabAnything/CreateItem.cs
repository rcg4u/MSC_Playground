using UnityEngine;

namespace GrabAnything
{
    public class CreateItem
    {
        private GrabAnything _grabAnything;

        public CreateItem(GrabAnything grabAnything)
        {
            _grabAnything = grabAnything;
        }

        internal void CheckColorName()
        {
            switch (_grabAnything._colour)
            {
                case 1:
                    _grabAnything._colourString = "Yellow";
                    _grabAnything._colorformat = "yellow>";
                    break;
                case 2:
                    _grabAnything._colourString = "Green";
                    _grabAnything._colorformat = "lime>";
                    break;
                case 3:
                    _grabAnything._colourString = "Blue";
                    _grabAnything._colorformat = "blue>";
                    break;
                case 4:
                    _grabAnything._colourString = "Magenta";
                    _grabAnything._colorformat = "magenta>";
                    break;
                case 5:
                    _grabAnything._colourString = "Red";
                    _grabAnything._colorformat = "red>";
                    break;
                case 6:
                    _grabAnything._colourString = "White";
                    _grabAnything._colorformat = "white>";
                    break;
                case 7:
                    _grabAnything._colourString = "Black";
                    _grabAnything._colorformat = "black>";
                    break;
                case 8:
                    _grabAnything._colourString = "Gray";
                    _grabAnything._colorformat = "grey>";
                    break;
                case 9:
                    _grabAnything._colourString = "Cyan";
                    _grabAnything._colorformat = "cyan>";
                    break;
                case 10:
                    _grabAnything._colourString = "Orange";
                    _grabAnything._colorformat = "orange>";
                    break;
                default:
                    _grabAnything._colourString = "Yellow";
                    _grabAnything._colorformat = "yellow>";
                    break;
            }
        }

        internal void SetColor(GameObject primitivGameObject)
        {
            switch (_grabAnything._colour)
            {
                case 1:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case 2:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    break;
                case 3:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                case 4:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
                    break;
                case 5:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case 6:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case 7:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                    break;
                case 8:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
                    break;
                case 9:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    break;
                case 10:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = new Color(0.9f, 0.3f, 0.0f);
                    break;
                default:
                    primitivGameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
            }
        }

        internal void CreatePrimitive(PrimitiveType primitive, string type)
        {
            var createdItem = GameObject.CreatePrimitive(primitive);
            createdItem.AddComponent<Rigidbody>();
            if (type == "board")
            {
                createdItem.name = "Board";
                createdItem.transform.localScale = new Vector3(0.1f, 2, 1);
            }
            SetColor(createdItem);
            createdItem.transform.position = _grabAnything._fpsCamera.transform.position + _grabAnything._fpsCamera.transform.forward * 2;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;


namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;
    using static System.Net.Mime.MediaTypeNames;

    public class DSNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DSChoiceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public string Speaker { get; set; }
        public string ImagePath { get; set; }
        public byte ImageData { get; set; }
        private Texture2D nodeTexture;



        public DSDialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        protected DSGraphView graphView;
        private Color defaultBackgroundColor;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public void SetImage(Texture2D texture)
        {
            nodeTexture = texture;
            // Update your node's visual representation with the texture
        }


        public virtual void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            DialogueName = nodeName;
            Choices = new List<DSChoiceSaveData>();
            Text = "Dialogue text.";
            Speaker = "Speaker";
            ImagePath = string.Empty;

            SetPosition(new Rect(position, Vector2.zero));

            graphView = dsGraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField)callback.target;

                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.NameErrorsAmount;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = target.value;

                    graphView.AddUngroupedNode(this);

                    return;
                }

                DSGroup currentGroup = Group;

                graphView.RemoveGroupedNode(this, Group);

                DialogueName = target.value;

                graphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            /* INPUT CONTAINER */

            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            // Existing foldout for Dialogue Text
            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            TextField textTextField = DSElementUtility.CreateTextArea(Text, null, callback => Text = callback.newValue);
            textTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__quote-text-field"
            );
            textFoldout.Add(textTextField);

            // Adding foldout for Speaker field
            Foldout nameFoldout = DSElementUtility.CreateFoldout("Speaker");
            TextField nameTextField = DSElementUtility.CreateTextArea(Speaker, null, callback => Speaker = callback.newValue);
            nameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__name-text-field"
            );
            nameFoldout.Add(nameTextField);

            // Add both foldouts to the customDataContainer
            customDataContainer.Add(textFoldout);
            customDataContainer.Add(nameFoldout);

            // Adding foldout for Image slot
            Foldout imageFoldout = DSElementUtility.CreateFoldout("Image Slot");

            // Create an Image element to display the image
            UnityEngine.UIElements.Image imageElement = new UnityEngine.UIElements.Image();
            imageElement.scaleMode = ScaleMode.ScaleToFit;
            imageElement.AddToClassList("ds-node__image");

            // Create a button to load/select the image
            Button loadImageButton = new Button(() =>
            {
                // Open the file picker using EditorUtility.OpenFilePanel
                string imagePath = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

                if (!string.IsNullOrEmpty(imagePath))
                {
                    var imageTexture = new Texture2D(2, 2);
                    imageTexture.LoadImage(System.IO.File.ReadAllBytes(imagePath)); // Load image from file

                    imageElement.image = imageTexture; // Assign the image to the element
                }
            })
            {
                text = "Load Image"
            };

            // Add the Image and Button to the foldout
            imageFoldout.Add(imageElement);
            imageFoldout.Add(loadImageButton);

            // Add the imageFoldout to the customDataContainer
            customDataContainer.Add(imageFoldout);

            // Add the customDataContainer to the extension container
            extensionContainer.Add(customDataContainer);



        }


        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextAsset InkFile;
    [SerializeField]private GameObject _textBox;
    public bool isTalking = false;

    private static Story _story;
    private TMPro.TMP_Text _nametag;
    private TMPro.TMP_Text _message;
    private List<string> _tags;


    private void Awake()
    {
        foreach (GameObject button in _choices)
        {
            button.SetActive(false);                //Desativa todos os botoes e tira da tela.
        }
    }

    private TextMeshProUGUI[] _choicesText;

    void Start()
    {
        _story = new Story(InkFile.text);                                            //pega o arquivo do ink e o deixa editavel/acessável no unity
        _nametag = _textBox.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();   //Pega os objetos que estão dentro do dialogue box na ordem e a caixa de texto do inspector respectivo
        _message = _textBox.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        _tags = new List<string>(); 




        _choicesText = new TextMeshProUGUI[_choices.Length];
        int index = 0;
        foreach(GameObject choice in _choices)                       //Pega o elemento de texto que ta dentro  do botao e coloca na variavel em cima
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_story.currentChoices.Count > 0)        //Para não entrar em loop infinito no advance dialogue e por consequencia no show choices
            {

            }
            else if (_story.canContinue)
            {
                AdvanceDialogue();
            }
            else
            {
                FinishDialogue();
            }


        }

    }



    private void ParseTags()
    {
        _tags = _story.currentTags;             //Define a lista de tag com as tags do ink (#), 
        foreach (string tag in _tags)           //Percorre a lista de tags 
        {
            string prefix = tag.Split(' ')[0]; 
            string param = tag.Split(' ')[1];    //Para cada Tag ele vai dividir entre prefixo e parametro(esquerda e direita). Cada tag tem a sua vez

            switch (prefix.ToLower())
            {
                case "name":                   //Verifica se este determinado string existe na tag. Se sim, rodar a linha de baixo
                    if (param == "empty")
                    {
                        _nametag.text = "";
                    }
                    else
                    {
                        _nametag.text = param;
                    }
                    break;

                case "narrator":
                    if (param == "empty")
                    {
                        _nametag.text = "Narrator";
                      
                    }
                    break;

                case "picture":


                    break;

                case "save":
                    DataManager.Instance.AddCondition(param,true);
                    break;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)           //Essa função espera receber uma string, e mostrar ela para o Sentence dentro do dialoguebox
    {
        _message.text = "";
        foreach (char letter in sentence.ToCharArray())          //Para fazer o texto aparecer letra a letra na frase
        {
            _message.text += letter;
            yield return null;
        }
        yield return null;
    }

    [Header("Choices UI")]
    [SerializeField] private GameObject[] _choices;             //cria uma lista de choices
    

    private void ShowChoices() 
    {
        List<Choice> currentChoices = _story.currentChoices;        //pega as opções de escolha do inky e coloca numa lista

        if (currentChoices.Count > _choices.Length)                 //só para caso a lista de escolhas seja maior do que a UI aguenta
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices give:" + currentChoices.Count);  
            return;
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);              //ativar botao
            _choicesText[index].text = choice.text;                  //colocar o texto da historia dentro do botão
            index++; 
        }


       for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);                //desativar botoes nao utilizados
        }

        StartCoroutine(SelectFirstChoice());

    }


    private IEnumerator SelectFirstChoice() 
    {

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);      //fazer as setas funcionaram
        AdvanceDialogue();

    }




    public static void SetDecision(int choiceIndex)             //para inserir o comando de clicar para escolher
    {
        _story.ChooseChoiceIndex(choiceIndex);
    }



    public void AdvanceDialogAfterChoice()                  //Mata os botoes depois de uma escolha ser feita
    {
        foreach (GameObject choiceGameObject in _choices)
        {
            choiceGameObject.SetActive(false);               //ativar botao
        }
        AdvanceDialogue();
    }







    private void AdvanceDialogue()
    {
        if (_story.canContinue)
        {
            string currentSentence = _story.Continue();     //Continua a historia, le as tags, para os ienumerators e começa a digitar o proximo texto
            ParseTags();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentSentence));
            if (_story.currentChoices.Count != 0)            //verifica se existem escolhas, e inicia a show choices
            {
                ShowChoices();
            }
        }
        

    }

    
   

   

    private void FinishDialogue()
    {
        SceneManager.LoadScene("LocationSelect");
    }

}

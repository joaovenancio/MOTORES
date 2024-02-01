using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public TextAsset InkFile;
    [SerializeField]private GameObject _textBox;
    //[SerializeField]private GameObject _customButton;
    //[SerializeField]private GameObject _optionPanel;
    public bool isTalking = false;

    private static Story _story;
    private TMPro.TMP_Text _nametag;
    private TMPro.TMP_Text _message;
    private List<string> _tags;
    //private static Choice _choiceSelected;
    





    void Start()
    {
        _story = new Story(InkFile.text); //pega o arquivo do ink e o deixa editavel/acessável no unity
        _nametag = _textBox.transform.GetChild(0).GetComponent<TMPro.TMP_Text>(); //Pega os objetos que estão dentro do dialogue box na ordem e a caixa de texto do inspector respectivo
        _message = _textBox.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        _tags = new List<string>(); 
        //_choiceSelected = null;




        _choicesText = new TextMeshProUGUI[_choices.Length];
        int index = 0;
        foreach(GameObject choice in _choices) //ADICIONADO 25/01 esse chunk aqui
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }






    private void ParseTags()
    {
        _tags = _story.currentTags; //Define a lista de tag com as tags do ink (#), 
        foreach (string tag in _tags) //Percorre a lista de tags 
        {
            string prefix = tag.Split(' ')[0]; 
            string param = tag.Split(' ')[1]; //Para cada Tag ele vai dividir entre prefixo e parametro(esquerda e direita). Cada tag tem a sua vez

            switch (prefix.ToLower())
            {
                case "name": //Verifica se este determinado string existe na tag. Se sim, rodar a linha de baixo
                    if (param == "empty")
                    {
                        _nametag.text = "";
                    }
                    else
                    {
                        _nametag.text = param;
                    }
                    break;
                case "color":
                    //SetTextColor(param);
                    break;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence) //Essa função espera receber uma string, e mostrar ela para o Sentence dentro do dialoguebox
    {
        _message.text = "";
        foreach (char letter in sentence.ToCharArray()) //Para fazer o texto aparecer letra a letra na frase
        {
            _message.text += letter;
            yield return null;
        }
        //CharacterScript tempSpeaker = GameObject.FindObjectOfType<CharacterScript>();  (ainda nao funciona)
        // if (tempSpeaker.isTalking)
        // {
        //     SetAnimation("idle");
        // }
        yield return null;
    }










    /*private IEnumerator ShowChoices()  ISSO DAQUI FUNCIONA SÓ TO TESTANDO
    {
        Debug.Log("There are choices to be made here");
        List<Choice> choices = _story.currentChoices; //pega a lista de escolhas e determina uma variavel pra ela

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject temp = Instantiate(_customButton, _optionPanel.transform); //cria um botao dentro do painel das escolhas
            temp.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = choices[i].text; //pega o texto que ta dentro do botão e insere a escolha nesse texto
            temp.AddComponent<Selectable>();
            temp.GetComponent<Selectable>().element = choices[i]; //Ele criou um objeto do tipo selectable (outro codigo), com 1 atributo e 1 metodo.
            temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); }); //VOLTAR AQUI DEPOIS AAAAAAAAAA
        }

        _optionPanel.SetActive(true); //aparece na tela

        yield return new WaitUntil(() => { return _choiceSelected != null; }); //espera ate o player escolher algo

        AdvanceFromDecision();

    }*/

    /*private void AdvanceFromDecision()
   {
       _optionPanel.SetActive(false);
       for (int i = 0; i < _optionPanel.transform.childCount; i++)
       {
           Destroy(_optionPanel.transform.GetChild(i).gameObject); //isso aqui é pra nao inundar a tela de opções
       }
       _choiceSelected = null;
       AdvanceDialogue();
   } */


    









    [Header("Choices UI")]
    [SerializeField] private GameObject[] _choices; //ADICIONADO 25/01 essas 3 linhas
    private TextMeshProUGUI[] _choicesText;

    private void ShowChoices() //ADICIONADO 25/01 esse chunk
    {
        List<Choice> currentChoices = _story.currentChoices;

        if (currentChoices.Count > _choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices give:" + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            _choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < _choices.Length; i++)
        {
            _choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());


    }


    private IEnumerator SelectFirstChoice()   //ADICIONADO 25/01 esse chunk
    {
        //clear first then wait
        //for at least one frame before we set the current selected object
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0].gameObject);
        AdvanceDialogue();
    }




    public static void SetDecision(int choiceIndex) //ADD 25/01
    {
        _story.ChooseChoiceIndex(choiceIndex);
    }











    private void AdvanceDialogue()
    {
        string currentSentence = _story.Continue();  //Continua a historia, le as tags, para os ienumerators e começa a digitar o proximo texto
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    
   

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Is there more to the story?
            if(_story.canContinue)
            {
                //_nametag.text = "Luke";
                AdvanceDialogue();
            } else
            {
                FinishDialogue();
            }
            Debug.Log(_story.currentChoices.Count);
            //Are there any choices?
            if(_story.currentChoices.Count != 0)
            {
                ShowChoices();  
            }
        }
       
    }

    private void FinishDialogue()
    {
        Debug.Log("The End");
    }

}

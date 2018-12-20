using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Main : MonoBehaviour {
    public static int scoreJ;
    public static int scoreIA;
    public static int tourDeJeu; //0 pour joueur, 1 pour IA

    //Variables utilisées pour l'affichage du score
    private string scoreJtext;
    private string scoreIAtext;
    public Text scoreText;

    //Variables utilisées pour l'apprentissage machine
    private bool apprentissageActive;//L'Apprentissage Machine est activé
    private bool apprentissageEnCours;//Pour n'avoir qu'un envoi à la fois vers l'IA

	void Start () {
        scoreJ = 0;
        scoreIA = 0;
        tourDeJeu = 0;
        apprentissageActive = false;
    }


    private void StartApprentissageIA()
    {
        StartCoroutine(ApprentissageIA());
    }
    private IEnumerator ApprentissageIA()
    {//on envoie des balles à trajectoire aléatoire sur le terrain de l'IA
        yield return new WaitUntil(() => Script_Balle.tableJ == true);

        GameObject.Find("Balle").transform.position = new Vector3(UnityEngine.Random.Range(-3f,3f), UnityEngine.Random.Range(2f,8f), UnityEngine.Random.Range(-4f,0f));
        Script_Balle.rb.velocity = new Vector3(UnityEngine.Random.Range(-2f,2f), 0f, UnityEngine.Random.Range(3f,9f));

        Script_Balle.tableIA = false;
        Script_Balle.raquetteJ = true;
        Script_Balle.tableJ = true;
        Script_Balle.raquetteIA = false;
        Script_Balle.dernierContact = false;

        yield return new WaitUntil(() => (Script_Balle.tableIA == true || Script_Balle.raquetteIA == true || Script_Balle.sol == true));

        apprentissageEnCours = false;
    }

    private void StartAffichageScore()
    {
        StartCoroutine(AffichageScore());
    }
    private IEnumerator AffichageScore()
    {
        yield return new WaitUntil(() => Script_Balle.raquetteIAatapeunefois == false);
        scoreJtext = scoreJ.ToString();
        scoreIAtext = scoreIA.ToString();
        scoreText.text = "Joueur : " + scoreJtext + " - IA : " + scoreIAtext;
    }

    
    void Update () {
        StartAffichageScore();

        //activer le mode apprentissage
        if(apprentissageActive == true && apprentissageEnCours == false)
        {
            apprentissageEnCours = true;
            StartApprentissageIA();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if(apprentissageActive == false)
            {
                apprentissageActive = true;
            }
            else
            {
                apprentissageActive = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class Script_RaquetteIA : MonoBehaviour
{//Le fichier se lit {posBalle,dirBalle,vitBalle,vitRaq,recomp}
    public Rigidbody rb;
    public Vector3 PositionBalle;
    public Vector3 DirectionBalle;
    public Vector3 VitesseBalle;
    public string tabBDD;
    private bool evenementEnCours;//Sert à sécuriser Update() afin de n'appeler BoucleEvenement() qu'une seule fois
    private bool evenementEstDejaDansFichier;//Sert de vérification pour ne pas sauvegarder plusieurs fois le même évènement dans le fichier

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        LectureBDD();
        evenementEnCours = false;
        evenementEstDejaDansFichier = false;
    }
	
    void Mise_A_Jour_Position(Vector3 PosBalleAvt)
    {//récupérer les positions/directions des attributs
        PositionBalle = GameObject.Find("Balle").transform.position;
        PositionBalle.x = Mathf.Round(PositionBalle.x);
        PositionBalle.y = Mathf.Round(PositionBalle.y);
        PositionBalle.z = Mathf.Round(PositionBalle.z);

        Vector3 PosBalleApr = GameObject.Find("Balle").transform.position;
        DirectionBalle = PosBalleApr - PosBalleAvt;//Obtenir le vecteur direction de la balle
        DirectionBalle.x = Mathf.Round(DirectionBalle.x);
        DirectionBalle.y = Mathf.Round(DirectionBalle.y);
        DirectionBalle.z = Mathf.Round(DirectionBalle.z);

        VitesseBalle = Script_Balle.vitesseBalle;
        VitesseBalle.x = Mathf.Round(VitesseBalle.x);
        VitesseBalle.y = Mathf.Round(VitesseBalle.y);
        VitesseBalle.z = Mathf.Round(VitesseBalle.z);
    }

    void StartBoucleEvenement()
    {
        StartCoroutine(BoucleEvenement());
    }
    private IEnumerator BoucleEvenement()
    {//fouille dans la DB puis applique ou réagis aléatoirement
        Vector3 velociteAppliquee = ComparaisonBDD();
        //Quand la balle touche la table de l'IA, l'IA applique son mouvement 
        yield return new WaitUntil(() => Script_Balle.tableIA == true);
        if(Script_Balle.tableIA == true)
        {
            rb.velocity = velociteAppliquee;
        }

        //En fonction du résultat de l'action de l'IA, on enregistre l'action soit dans tabBDD4, soit dans tabBDD3
        yield return new WaitUntil(() => ((Script_Balle.sol == true) || (Script_Balle.raquetteJ == true) || (Script_Balle.tableJ)));
        if((Script_Balle.tableJ == true || Script_Balle.raquetteJ == true || Script_Balle.sol == true) && Script_Balle.raquetteIAatapeunefois == true)
        {
            print("renvoi parfait");
            if (evenementEstDejaDansFichier == false)
            {
                EcritureBDD(velociteAppliquee, 4);
            }
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            rb.position = new Vector3(0,2,10);
            evenementEnCours = false;
            evenementEstDejaDansFichier = false;
            Script_Balle.raquetteIAatapeunefois = false;
        }
        else
        {
            print("renvoi raté");
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            rb.position = new Vector3(0, 2, 10);
            evenementEnCours = false;
        }
    }

    void LectureBDD()
    {//fouille dans le fichier de BDD externe (au lancement du jeu)
        TextAsset txtAsset = (TextAsset)Resources.Load("test", typeof(TextAsset));
        string texteComplet = txtAsset.text;
        string textePartiel;
        int n = 0;
        int accolade1 = 0;
        int recompNb = 0;
        while (n != texteComplet.Length)
        {
            if(texteComplet[n] == '{')
            {//début évènement
                accolade1 = n;
            }
            if(texteComplet[n] == '}')
            {//fin évènement
                recompNb = texteComplet[n - 1];
                textePartiel = "";
                for(int i=accolade1;i<=n;i++)
                {
                    textePartiel = textePartiel + texteComplet[i];
                }
                if (recompNb == '4') tabBDD = tabBDD + textePartiel;
                if (recompNb == '3') tabBDD = tabBDD + textePartiel;
                if (recompNb == '2') tabBDD = tabBDD + textePartiel;
                if (recompNb == '1') tabBDD = tabBDD + textePartiel;
            }
            n++;
        }
    }

    Vector3 ComparaisonBDD()
    {//regarde si l'action en cours n'est pas déjà notée dans les BDD, et sinon donne des valeurs randoms
        int n = 0;
        string[] tabX = new string[4];//les tableaux qui vont contenir les XYZ en string des BDDs
        string[] tabY = new string[4];
        string[] tabZ = new string[4];
        string tabRecomp = "";
        int[] tabNumX = new int[4];//les tableaux qui vont contenir les XYZ en float des BDDs
        int[] tabNumY = new int[4];
        int[] tabNumZ = new int[4];
        int tabNumRecomp;
        Vector3 PositionBalle_Evenement;//les Vector3 correspondant à un évènement de la BDD, qu'on compare avec l'action en cours
        Vector3 DirectionBalle_Evenement;
        Vector3 VitesseBalle_Evenement;
        Vector3 VitesseRaquette_Evenement;

        while(n != tabBDD.Length)//La boucle qui va parcourir tous les évènements du fichier, comparer avec l'action en cours, et renvoyer cet évènement s'il est concluant
        {
            if(tabBDD[n] == '{')
            {//Prendre UN évènement
                n++;
                for (int i = 0; i < 12; i++)
                {//(x,y,z)
                    while(tabBDD[n] != ';')
                    {//x
                        if ((i % 3) == 0) { tabX[i/3] = tabX[i/3] + tabBDD[n]; } 
                        if ((i % 3) == 1) { tabY[i/3] = tabY[i/3] + tabBDD[n]; }
                        if ((i % 3) == 2) { tabZ[i/3] = tabZ[i/3] + tabBDD[n]; }
                        n++;                   

                    }//y
                    n++;
                }
                while(tabBDD[n] != '}')
                {//n
                    tabRecomp = tabRecomp + tabBDD[n];
                    n++;
                }
            }
            n++;


            /*for (int i = 0; i < 4; i++)//POUR TESTER LES TABLEAUX
            {
                print("tabX vaut " + tabX[i]);
                print("tabY vaut " + tabY[i]);
                print("tabZ vaut " + tabZ[i]);
            }*/


            //on place les XYZ des tableaux Str dans les tableaux Num
            for (int i=0;i<4;i++)
            {
                tabNumX[i] = int.Parse(tabX[i]);
                tabNumY[i] = int.Parse(tabY[i]);
                tabNumZ[i] = int.Parse(tabZ[i]);
            }
            tabNumRecomp = int.Parse(tabRecomp);

            
            //Comparer l'action en cours avec l'évènement tiré de la BDD
            PositionBalle_Evenement = new Vector3(tabNumX[0],tabNumY[0],tabNumZ[0]);
            DirectionBalle_Evenement = new Vector3(tabNumX[1], tabNumY[1],tabNumZ[1]);
            VitesseBalle_Evenement = new Vector3(tabNumX[2],tabNumY[2],tabNumZ[2]);
            VitesseRaquette_Evenement = new Vector3(tabNumX[3], tabNumY[3], tabNumZ[3]);


            //si un évènement parfait similaire est déjà stocké dans la BDD, on le reproduit
            if ((PositionBalle_Evenement.Equals(PositionBalle))&&(DirectionBalle_Evenement.Equals(DirectionBalle))&&(VitesseBalle_Evenement.Equals(VitesseBalle)))
            {
                if(tabNumRecomp == 4)
                {
                    print("La comparaison évènement/BDD a marché !");
                    evenementEstDejaDansFichier = true;
                    return VitesseRaquette_Evenement;
                }
            }


            //Vider les tableaux pour pouvoir capturer un nouvel évènement
            for(int i=0;i<4; i++)
            {
                tabX[i] = "";
                tabY[i] = "";
                tabZ[i] = "";
                tabRecomp = "";
            }
        }
        //Si on ne trouve pas d'équivalent dans les BDD, on donne des valeurs randoms
        return new Vector3(Mathf.Round(UnityEngine.Random.Range(-6f, 6f)), 2.0f, Mathf.Round(UnityEngine.Random.Range(-12f, 0f)));
    }

    void EcritureBDD(Vector3 vitRaq, int recomp)
    {//enregistre l'action dans la DB : {(posBalle),(dirBalle),(vitBalle),(dirRaq),(vitRaq),n)
        TextWriter writer;
        string fileName = "./Assets/Resources/test.txt";
        writer = File.AppendText(fileName);
        string donnee = "{" + PositionBalle.x + ';' + PositionBalle.y + ';' + PositionBalle.z + ';'
                  + DirectionBalle.x + ';' + DirectionBalle.y + ';' + DirectionBalle.z + ';'
                  + VitesseBalle.x + ';' + VitesseBalle.y + ';' + VitesseBalle.z + ';' 
                  + vitRaq.x + ';' + vitRaq.y + ';' + vitRaq.z + ';' 
                  + recomp + '}';
        tabBDD = tabBDD + donnee;//ajouter directement dans la string tabBDD4, pour ne pas avoir à relancer le programme pour prendre en compte les modifs
        writer.Write(donnee);
        writer.Close();
    }

	void Update ()
    {
        if((Script_Balle.tableIA == true ) && (evenementEnCours==false))
        {//Quand la balle passe au dessus du filet
            evenementEnCours = true;
            Vector3 PosBalleAvant = GameObject.Find("Balle").transform.position;
            Mise_A_Jour_Position(PosBalleAvant);
            StartBoucleEvenement();
        }
	}
}


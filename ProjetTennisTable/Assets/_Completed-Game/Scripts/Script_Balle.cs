using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Balle : MonoBehaviour {
    
    public static Rigidbody rb;
    public static Vector3 vitesseBalle;
    public static bool tableJ;//True si la balle a déjà rebondi une fois dans mon camp, False sinon
    public static bool raquetteJ;//True si la raquette du joueur a déjà touché la balle, False sinon
    public static bool tableIA;
    public static bool raquetteIA;
    public static bool dernierContact;//0 si Joueur, 1 si IA
    public static bool sol;//faux la plupart du temps, passe à true quand la balle touche le sol (mais repasse false très rapidement après)
    public static bool raquetteIAatapeunefois;//Sert à faire fonctionner correctement les deux yield dans BoucleEvenement() de Scripta_RaquetteIA
    
	void Start () {
        rb = GetComponent<Rigidbody>();
        tableJ = false;
        raquetteJ = false;
        tableIA = false;
        raquetteIA = false;
        sol = false;
        raquetteIAatapeunefois = false;
	}
    
    void Balle_RemiseEnJeu(bool gagnant)
    {
        if(gagnant == false)
        {
            Script_Main.scoreJ += 1;
            rb.position = new Vector3(0, 6, -6);
            dernierContact = false;
            print("But du joueur");
        }
        if(gagnant == true)
        {
            Script_Main.scoreIA += 1;
            rb.position = new Vector3(0, 6, 6);
            dernierContact = true;
            print("But de l'IA");
        }
        rb.velocity = new Vector3(0, 0, 0);
        tableJ = false;
        raquetteJ = false;
        tableIA = false;
        raquetteIA = false;
        sol = false;
        raquetteIAatapeunefois = false;
    }


    void OnCollisionEnter(Collision col)
    {//gère les collisions de la balle avec la table/les raquettes/le filet
        if(col.gameObject.name == "Sol")
        {//si la balle sort du terrain
            sol = true;

            if(dernierContact == false)//si le joueur est le dernier a avoir touché la balle
            {
                if (tableJ == false && raquetteJ == false && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 1");
                }

                if (tableJ == false && raquetteJ == false && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 2");
                }

                if (tableJ == false && raquetteJ == false && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(false);
                    //print("Le joueur envoie la balle qui touche tableIA puis tombe sur le sol");
                }

                if (tableJ == false && raquetteJ == false && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(false);
                    print("Collision Interdite 4");
                }

                if (tableJ == false && raquetteJ == true && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("La balle renvoyé par raqJ n'a pas touché tableIA");
                }

                if (tableJ == false && raquetteJ == true && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(false);
                    print("Collision Interdite 6");
                }

                if (tableJ == false && raquetteJ == true && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(false);
                    //print("Le joueur vient de marquer sans toucher raqIA");
                }

                if (tableJ == false && raquetteJ == true && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 8");
                }

                if (tableJ == true && raquetteJ == false && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("Dernier contact avec tableJ mais aucun contact avec raquette");
                }

                if (tableJ == true && raquetteJ == false && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 10");
                }

                if (tableJ == true && raquetteJ == false && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("tableIA et tableJ sont true, les deux raq sont false");
                }

                if (tableJ == true && raquetteJ == false && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 12");
                }

                if (tableJ == true && raquetteJ == true && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("La raquette/table du joueur a touché la balle mais pas la table/raq de l'IA");
                }

                if (tableJ == true && raquetteJ == true && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 14");
                }

                if (tableJ == true && raquetteJ == true && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("Erreur tableJ et tableIA tous les deux True au même moment");
                }

                if (tableJ == true && raquetteJ == true && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 16");
                }
            }

            else //si l'IA est la dernière à toucher la balle
            {
                if (tableJ == false && raquetteJ == false && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 17");
                }

                if (tableJ == false && raquetteJ == false && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(false);
                    //print("raqIA touche la balle mais la renvoie sur le sol");
                }

                if (tableJ == false && raquetteJ == false && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(false);
                    //print("la balle ne touche aucune raquette, et tableIA. Collision Interdite 19");
                }

                if (tableJ == false && raquetteJ == false && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(false);
                    //print("raqIA touche la balle mais la renvoie dans le vide");
                }

                if (tableJ == false && raquetteJ == true && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 21");
                }

                if (tableJ == false && raquetteJ == true && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(false);
                    print("Collision Interdite 22");
                }

                if (tableJ == false && raquetteJ == true && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(false);
                    print("Collision Interdite 23");
                }

                if (tableJ == false && raquetteJ == true && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 24");
                }

                if (tableJ == true && raquetteJ == false && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("la balle touche tableJ sans toucher aucune raquette. Collision Interdite 25");
                }

                if (tableJ == true && raquetteJ == false && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    //print("la balle est renvoyée par raqIA sur tableJ, puis touche le sol");
                }

                if (tableJ == true && raquetteJ == false && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    //print("les deux tables sont à True. Collision Interdite 27");
                }

                if (tableJ == true && raquetteJ == false && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    //print("les deux tables sont à True. Collision Interdite 28");
                }

                if (tableJ == true && raquetteJ == true && tableIA == false && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 29");
                }

                if (tableJ == true && raquetteJ == true && tableIA == false && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 30");
                }

                if (tableJ == true && raquetteJ == true && tableIA == true && raquetteIA == false)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 31");
                }

                if (tableJ == true && raquetteJ == true && tableIA == true && raquetteIA == true)
                {
                    Balle_RemiseEnJeu(true);
                    print("Collision Interdite 32");
                }
            }
        }

        if(col.gameObject.name == "PlateauJoueur")
        {
            if(tableJ == true)
            {
                Balle_RemiseEnJeu(true);
            }
            raquetteIA = false;
            tableIA = false;
            tableJ = true;
        }

        if(col.gameObject.name == "RaquetteJ")
        {
            if(raquetteJ == true || raquetteIA == true)
            {
                Balle_RemiseEnJeu(true);
            }
            tableIA = false;
            raquetteJ = true;
            tableJ = true;
            raquetteIA = false;
            dernierContact = false;
        }

        if (col.gameObject.name == "PlateauIA")
        {
            if (tableIA == true)
            {
                Balle_RemiseEnJeu(false);
            }
            raquetteJ = false;
            tableJ = false;
            tableIA = true;
        }

        if (col.gameObject.name == "RaquetteIA")
        {
            if (raquetteIA == true || raquetteJ == true)
            {
                Balle_RemiseEnJeu(false);
            }
            tableIA = true;
            raquetteJ = false;
            tableJ = false;
            raquetteIA = true;
            dernierContact = true;
            raquetteIAatapeunefois = true;
        }
    }

	void FixedUpdate () {
        vitesseBalle = rb.velocity;
	}
}

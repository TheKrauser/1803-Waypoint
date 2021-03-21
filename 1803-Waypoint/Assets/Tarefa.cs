using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarefa : MonoBehaviour
{
    //Array para os GameObject que serão os Waypoints onde o player vai se dirigir
    public GameObject[] waypoints;

    //Ponto atual no qual o jogador está se dirigindo (Inicia em 0 pois Arrays e Lists começam na posição 0)
    int currentWP = 0;

    //Velocidade do player
    [Range(1, 10)]
    public int speed = 1;

    //Se o player ficar a uma distancia menor do que 1f um novo waypoint será atribuido para ele ir em direção
    [Range(1, 10)]
    public float accuracy = 1f;

    //Velocidade de rotação do player
    [Range(0.1f, 5)]
    public float rotSpeed = 0.4f;

    void Start()
    {
        //Encontra todos os GameObjects com a Tag "Waypoint" e joga na Array
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    void LateUpdate()
    {
        //Se o tamanho da Array for 0 então não faça nada, pois haverá apenas 1 waypoint
        if (waypoints.Length == 0)
            return;

        //Vetor que guarda a posição do waypoint atual nas coordenadas x e z, não levando em conta o y
        //Será usado para fazer o player olhar para esse waypoint
        Vector3 lookAtGoal = new Vector3(waypoints[currentWP].transform.position.x, this.transform.position.y, waypoints[currentWP].transform.position.z);

        //Vetor direção, fazendo o alvo (waypoint) menos a posição atual do jogador (transform.position)
        Vector3 direction = lookAtGoal - transform.position;

        //Ajusta a rotação do player usando Quaternion.Slerp, que fará uma transição suave entre o ponto A (transform.position) e o ponto B (direction)
        //Que é a posição atual do waypoint que o player está se dirigindo
        transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);

        //Se a magnitude da direção for menor que o accuracy (distância minima) será calculado um novo waypoint
        if (direction.magnitude < accuracy)
        {
            //Próximo waypoint do Array
            currentWP++;

            //Se o próximo waypoint não existir (for além do tamanho do Array) então volta para a posição 0
            if (currentWP >= waypoints.Length)
            {
                //Seta o valor para 0
                currentWP = 0;
            }
        }

        //Move o player para frente constantemente baseado na speed, como estará sempre olhando para o waypoint alvo, irá em direção a ele
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
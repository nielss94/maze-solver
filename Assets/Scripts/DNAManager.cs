using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DNAManager : MonoBehaviour 
{    
    [Header("Challenger details")]
    public Challenger challengerPrefab;
    public int amountOfChallengers = 1;
    public int genesLength = 14;

    private System.Random random;

    [SerializeField]private List<Challenger> population = new List<Challenger>();
    public int Generation;
    public float BestFitness;
    public Direction[] BestGenes;
    
    public float MutationRate;

    private float fitnessSum;

    private void Start() {
        random = new System.Random();
        BestGenes = new Direction[genesLength];
        
        for (int i = 0; i < amountOfChallengers; i++)
        {
            Challenger c = Instantiate(challengerPrefab, challengerPrefab.transform.position, Quaternion.identity);
            c.dna = new DNA<Direction>(genesLength, random, GetRandomDirection, FitnessFunction, true);
            c.transform.name = "Challenger " + i;
            population.Add(c);
        }

        InvokeRepeating("NewGeneration", 1, 2f);
    }


    public void NewGeneration()
    {
        if(population.Count <= 0)
        {
            return;
        }

        CalculateFitness();

        List<Challenger> newPopulation = new List<Challenger>();

        // for (int i = 0; i < amountOfChallengers; i++)
        // {
        //     Challenger c = Instantiate(challengerPrefab, challengerPrefab.transform.position, Quaternion.identity);
        //     c.dna = new DNA<Direction>(genesLength, random, GetRandomDirection, FitnessFunction, true);
        //     c.transform.name = "Challenger " + i;
        //     newPopulation.Add(c);
        // }

        //crossovers 
        for (int i = 0; i < population.Count; i++)
        {
            DNA<Direction> parent1 = ChooseParent();
            DNA<Direction> parent2 = ChooseParent();
            DNA<Direction> child = null;

            Challenger c = Instantiate(challengerPrefab, challengerPrefab.transform.position, Quaternion.identity);
            if(parent1 != null && parent2 != null)
            {
                child = parent1.CrossOver(parent2);
                child.Mutate(MutationRate);
                c.dna = child;
            }else{
                c.dna = new DNA<Direction>(genesLength, random, GetRandomDirection, FitnessFunction, true);
            }
            newPopulation.Add(c);
        }

        foreach (var item in population)
        {
            Destroy(item.gameObject);
        }
        population.Clear();
        population = newPopulation;

        Generation++;
    }

    public void CalculateFitness()
    {
        fitnessSum = 0;

        DNA<Direction> best = population[0].dna;

        for (int i = 0; i < population.Count; i++)
        {
            fitnessSum += population[i].dna.CalculateFitness(i);

            if(population[i].dna.Fitness > best.Fitness)
            {
                best = population[i].dna;
            }
        }

        BestFitness = best.Fitness;
        best.Genes.CopyTo(BestGenes, 0);
    }

    private DNA<Direction> ChooseParent()
    {
        double randomNumber = random.NextDouble() * fitnessSum;

        for (int i = 0; i < population.Count; i++)
        {
            if(randomNumber < population[i].dna.Fitness)
            {
                return population[i].dna;
            }

            randomNumber -= population[i].dna.Fitness;
        }

        return null;
    }

    private Direction GetRandomDirection()
    {
        int rand = Random.Range(0, 4);
        return (Direction)rand;
    }

    private float FitnessFunction(int index)
    {
        float score = 0;
        Challenger challenger = population[index];

        float distToStart = Vector3.Distance(challenger.transform.position, new Vector3(-1.5f, 0.5f, -1.042578f));

        float distToFinish = Vector3.Distance(challenger.transform.position, GameObject.FindGameObjectWithTag("Finish").transform.position);

        score += (distToStart - distToFinish);

        return score;
    }

    public void Winner(Challenger challenger)
    {
        challenger.dna.CalculateFitness(population.IndexOf(challenger));
        Time.timeScale = 0;
        this.enabled = false;
    }
}
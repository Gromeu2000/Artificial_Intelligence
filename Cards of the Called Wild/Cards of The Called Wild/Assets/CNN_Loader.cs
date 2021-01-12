using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class CNN_Loader : MonoBehaviour
{
    public NNModel modelData;
    private Model model;
    private IWorker worker;
    private Tensor input;
    private Tensor input2;
    // Start is called before the first frame update
    void Start()
    {
        
        //model = ModelLoader.LoadFromStreamingAssets(modelName + ".nn");
        model=ModelLoader.Load(modelData);

        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputeRef, model);

        input = new Tensor(0, 256, 256, 3);
       

        
        
    }

    // Update is called once per frame
    void Update()
    {
        //input = ExecuteParts(worker,input,5);
        //worker= worker.Execute(input);
        //input= worker.PeekOutput();
        //int A = 0;
    }

    Tensor ExecuteParts(IWorker worker,Tensor tense,int sync = 5)
    {

        var executor = worker.StartManualSchedule(tense);
        var it = 0;
        bool hasMoreWork;

        do
        {
            hasMoreWork = executor.MoveNext();
            if (++it % sync == 0)
            {
                worker.FlushSchedule();
            }

        } while (hasMoreWork);


        return worker.CopyOutput();
    }
}

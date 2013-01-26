using UnityEngine;

public class VisualizeAudio : MonoBehaviour
{
    void Update()
    {
        float[] spectrum = audio.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
        int i = 1;
        while (i < 1023)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
            i++;
        }
    }
}

//using BPMDetect;
//using NAudio;
//using NAudio.Wave;
//using System.Collections;

//public class TestScript : MonoBehaviour {

//    IWavePlayer waveOutDevice;
//    WaveStream mainOutputStream;
//    WaveChannel32 volumeStream;

//    // Use this for initialization
//    void Start () {
//        BPMDetection foo = new BPMDetection();



//        waveOutDevice = new WaveOut();

        

//        mainOutputStream = CreateInputStream("C:\\Users\\Alex\\Music\\GOTAudio\\1 - A Game of Thrones\\01 - The Hedge Knight.mp3");

//        waveOutDevice.Init(mainOutputStream);
//        waveOutDevice.Play();
//    }

//    private WaveStream CreateInputStream(string fileName)
//    {
//        WaveChannel32 inputStream = null;
//        if (fileName.EndsWith(".mp3"))
//        {
//            WaveStream mp3Reader = new Mp3FileReader(fileName);
//            inputStream = new WaveChannel32(mp3Reader);
//        }
//        else
//        {
//            //throw new InvalidOperationException("Unsupported extension");
//        }
//        volumeStream = inputStream;
//        return volumeStream;
//    }

//    // Update is called once per frame
//    void Update () {
	
//    }
//}

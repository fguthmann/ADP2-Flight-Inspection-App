using ex1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex1.ViewModel
{

    class FlightInfoViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private FGClient fg_client;

        // Notify that an element has been update to update all element
        public FlightInfoViewModel(FGClient fg_client)
        {
            this.fg_client = fg_client;
            fg_client.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        // Call the function that need to be update
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        // Update the values of the different element when the frame is update
        public float VM_CurrentFrame
        {
            set
            {
                _ = VM_Altimeter;
                _ = VM_AirSpeed;
                _ = VM_VerticalSpeed;
                _ = VM_GroundSpeed;
                _ = VM_Roll;
                _ = VM_Pitch;
                _ = VM_Yaw;
                _ = VM_Aileron;
                _ = VM_Elevator;
                _ = VM_Throttle;
                _ = VM_Throttle;

            }
        }
       
        //Get the new value of the general information to show it where needed
        public float VM_Altimeter { get { return fg_client.GetElement("altimeter", fg_client.CurrenFrame); } }
        public float VM_AirSpeed { get { return fg_client.GetElement("airspeed", fg_client.CurrenFrame); } }
        public float VM_VerticalSpeed { get { return fg_client.GetElement("verticalspeed", fg_client.CurrenFrame); } }
        public float VM_GroundSpeed { get { return fg_client.GetElement("groundspeed", fg_client.CurrenFrame); } }
        public float VM_Roll { get { return fg_client.GetElement("roll", fg_client.CurrenFrame); } }
        public float VM_Pitch { get { return fg_client.GetElement("pitch", fg_client.CurrenFrame); } }
        public float VM_Yaw { get { return fg_client.GetElement("yaw", fg_client.CurrenFrame); } }
        public float VM_Aileron { get { return fg_client.GetElement("aileron", fg_client.CurrenFrame); } }
        public float VM_Elevator { get { return fg_client.GetElement("elevator", fg_client.CurrenFrame); } }
        public float VM_Throttle { get { return fg_client.GetElement("throttle", fg_client.CurrenFrame); } }
        public float VM_Rudder { get { return fg_client.GetElement("rudder", fg_client.CurrenFrame); } }

        // Call the close function to close the program
        public void VM_close()
        {
            fg_client.Close();
        }

        // Slow forward plus the flight
        public void VM_SlowForwardPlus()
        {
            fg_client.CurrenFrame -= 2;
        }

        // Slow forward the flight
        public void VM_SlowForward()
        {
            fg_client.CurrenFrame -= 1;
        }

        // Play the flight
        public void VM_Play()
        {
            fg_client.Pause = false;
        }

        // Pause the flight
        public void VM_Pause()
        {
            fg_client.Pause = true;
        }
        // Restart the flight from beginning
        public void VM_Stop()
        {
            fg_client.CurrenFrame = 0;

        }

        // Fast forward the flight
        public void VM_FatsForward()
        {
            fg_client.CurrenFrame += 1;
        }

        // Fast forward plus the flight
        public void VM_FastForwardPlus()
        {
            fg_client.CurrenFrame += 2;

        }

        // Play the current time in second
        public int VM_CurrentTime(int currentFrame)
        {
            return currentFrame * 100;
        }

        // Show the current speed
        public double VM_CurrentSpeed(int framesPerSecond)
        {
            return framesPerSecond / 10.0;
        }


    }
}

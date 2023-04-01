import socket
import time
import numpy as np
import cv2
from threading import Thread


blink_detection_result = None 
face_detected = False


class Unity:

    def __init__(self):
        try:
            host, port = "127.0.0.1", 25000
            self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.sock.connect((host, port))
            self.connected_to_unity = True 
            print("[UNITY] connected")
        except:
            print("[UNITY] error-unable to connect to server")
            self.connected_to_unity = False


    def Send_To_C(self,  msg):
        if(self.connected_to_unity):
            self.sock.sendall(msg.encode("UTF-8"))
        else:
            print("[UNITY] error-client isnt connected to server, unable to send msg")

    def Disconnect(self):
        if(self.connected_to_unity):
            disconnect_message = "!DISCONNECT!"
            self.Send_To_C(disconnect_message)
            time.sleep(1)
            self.sock.close()
            self.Update_Connected_Unity(False)
            print("[UNITY] disconnected")
            exit()
        else:
            print("[UNITY] error-client isnt connected to server")

    def Is_Connected_Unity(self):
        return 
    
    def Update_Connected_Unity(self, x):
        self.connected_to_unity = x

    



        
if __name__ == "__main__":
        # set up connection to unity
        unity = Unity()

        
        #Initializing the face and eye cascade classifiers from xml files
        face_cascade = cv2.CascadeClassifier('haarcascade_frontalface_default.xml')
        eye_cascade = cv2.CascadeClassifier('haarcascade_eye_tree_eyeglasses.xml')
        
        #Variable store execution state
        first_read = True
        result =  None
        previous_result = None 
        blink_num = 0
        
        #Starting the video capture
        cap = cv2.VideoCapture(0)
        ret,img = cap.read()        

        while(ret):
            ret,img = cap.read()
            #Converting the recorded image to grayscale
            gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            #Applying filter to remove impurities
            gray = cv2.bilateralFilter(gray,5,1,1)

            
        
            #Detecting the face for region of image to be fed to eye classifier
            faces = face_cascade.detectMultiScale(gray, 1.3, 5,minSize=(200,200))
            if(len(faces)>0):
                for (x,y,w,h) in faces:
                    face_detected = True
                    img = cv2.rectangle(img,(x,y),(x+w,y+h),(0,255,0),2)
        
                    #roi_face is face which is input to eye classifier
                    roi_face = gray[y:y+h,x:x+w]
                    roi_face_clr = img[y:y+h,x:x+w]
                    eyes = eye_cascade.detectMultiScale(roi_face,1.3,5,minSize=(50,50))
        
                    #Examining the length of eyes object for eyes
                    if(len(eyes)>=2): 

                        if(not first_read):
                            eye_open = True
                            cv2.putText(img,
                            "Eyes open!", (70,70),
                            cv2.FONT_HERSHEY_PLAIN, 2,
                            (255,255,255),2)
                            result = "Eyes Open!"
                            

                    else:
                        if(first_read):
                            eye_open = False
                            #To ensure if the eyes are present before starting
                            cv2.putText(img,
                            "No eyes detected", (70,70),
                            cv2.FONT_HERSHEY_PLAIN, 3,
                            (0,0,255),2)
                            result = "No eyes detected"
                        else:
                            result = "Blink"
                            first_read=True




            else:
                cv2.putText(img,
                "No face detected",(100,100),
                cv2.FONT_HERSHEY_PLAIN, 3,
                (0,255,0),2)
                face_detected = False
        
            #Controlling the algorithm with keys
            cv2.imshow('img',img)
            a = cv2.waitKey(1)
            ##disconnects unity when "q" is pressed
            if(a==ord('q')):
                unity.Disconnect()
                break

            ##checks for a blink
            if(result == "Eyes Open!" and previous_result == "Blink"):
                blink_num += 1
                ##sends blink number to unity
                if(blink_num == 1):
                    unity.Send_To_C("-1") 
                else:
                    unity.Send_To_C(str(blink_num))



            first_read = False
            previous_result = result
        
        cap.release()
        cv2.destroyAllWindows()
        



    
    

    #blink_detection()



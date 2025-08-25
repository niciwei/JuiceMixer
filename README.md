# VR Juice Mixer 
A VR training and simulation scenario featuring an interactive juice mixing machine. Built with Unity and the XR Interaction Toolkit, this project provides interactive machine parts, process simulation, and an MQTT connectivity example
 - Unity Version: 2022.3.46f1
 - Interaction Toolkit: 2.6.3 (Starter Assets, Device Simulator, Hands Interaction Demo (if needed))


##  Features  
- **Juice Mixer Model**  
- **Interactive machine parts** via XR Interaction Toolkit  
- **Simulation** of juice filling and pump functionality  
- **MQTT connection example** for data exchange and external integrations  

## XR Interaction Toolkit Usage  
- **Direct & Ray Interactor** for near/far interactions  
- **Grab Interactables** to manipulate machine parts  
- **Sockets** for snapping parts into place (containers, lids, tubes, etc.)  

##  Juice Mixer Components  

- **Control Bar**  
  - Toggle pumps on/off  
  - Adjust pump strength  

- **Bases**  
  - 3 sizes: Small (S), Medium (M), Large (L)  
  - Used as bases for containers  

- **Containers**  
  - 3 sizes: S, M, L  
  - Can be filled with liquid  

- **Container Lids**  
  - Fit onto containers  
  - 3 holes for:  
    - pH sensor  
    - Temperature sensor  
    - Tube connection  

- **Sensors**  
  - pH and temperature sensors for each container/pump  

- **Pumps & Tubes**  
  - 6 tubes connect containers to the final container via pumps  

- **End Result Container**  
  - Collects the final mixed juice  
  - Connected to all 6 tubes  

##  Juice Fill Station  
- Fill containers automatically by holding them under the barrels  
- Empty containers automatically at the sink  

##  Spare Part Station  
- Contains extra bases, containers, and lids in all sizes  

## Scenes  
- **JuiceMixer**  
  - Base simulation scene  
  - Juice mixer parts (containers, lids, tubes) must be assembled manually  
  - Pump functionality is simulated  

- **JuiceMixerConnected**  
  - Includes MQTT connectivity example  
  - Demo visualizations for sensor data  


##  Assets Used  

We used free assets to build the scene:  
- furniture (main scene):  https://assetstore.unity.com/packages/3d/props/furniture/furniture-free-low-poly-3d-models-pack-260522  
- garage: https://assetstore.unity.com/packages/3d/props/interior/simple-garage-197251



##  MQTT Example  

- Example setup included in `JuiceMixerConnected` scene  
- Demonstrates external sensor data integration and visualization  

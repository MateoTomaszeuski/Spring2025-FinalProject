<div style="text-align: right"> Date </div>

# Weekly Report X

## Progress Summary
### Scope Progress
Progress on scope (show what you've done, what remains)

### Learning Objectives Progress
Progress on learning objectives (note which objectives you can demonstrate through this project, which you cannot)

---

## Work Logs
These time logs outline our meetings spent mob programming, debugging, and planning as a group.

[Insert Time Logs Here]

---
## Blocking Issues
Share 3+ different blocking issues that you were stuck on for a while, and how you resolved them.

#### 1. Deploying API to Azure
* **Description:** 
  - Deployment failed after modifying the default workflow file and removing artifacts.
* **Attempts To Fix:**
  - Tried printing the working directory in the pipeline. That and reading the errors revealed that it wasn't finding the right dll file.
* **Solution:**
  - Added the path to "myapp" to the `package` in the deployment step.

#### 2. Integration tests don't run in workflow
* **Description:** 
  - Integration tests fail every time, even in the correct directory.
* **Attempts To Fix:**
  - Tried running 'ls' in the Dockerfile, and saw that we were in the correct directory.
* **Solution:**
  - Read the error message again and realized that the T-Unit test project was using net8.0, and the rest of our project was using net9.0. Changing it to 9.0 solved the problem.

#### 3. Connection refused in integration tests
* **Description:** 
  - In our first integration tests, we tried to create an HTTP Client to access the API on localhost port 8080. When we tried to get a result from the enpdoint, it says "connection refused."
* **Attempts To Fix:**
  - Added a logging step to the docker-compose to see the startup logs for the API container. The logs didn't actually print in our workflow logs. 
* **Solution:**
  - Realized our target URI was https instead of http. Also, the API's appsettings.json needed to have 0.0.0.0 instead of localhost. Then, we could access the API being run in the docker compose.
  
#### 4. Testing MAUI in Deployment Pipeline
* **Description:** 
  - The MAUI project gets recognized as a class library when `dotnet test` gets run in the pipeline. It always results in a failing test, failing the pipeline.
* **Attempts To Fix:**
  - Looked for a docker image that could be used to run the tests correctly. Most of them were for the Windows OS, but we found one for Linux.
* **Solution:**
  - Escalated to Alex, who confirmed that we should separate our viewmodel logic into a shared project and test that instead of the MAUI project.


#### 5. Controller Routing
* **Description:** 
  - Can't assign additional routes on top of the default controller route.
* **Attempts To Fix:**
  - Tried using [Route("update")], but that caused problems in Swagger.
* **Solution:**
  - Just a syntax problem with slashes. Also realized we didn't need a separate attribute for the path.

#### 6. Calling Async Methods in Constructor (Without Warnings)
* **Description:** 
  - We can't have any warnings (for our deployment pipeline), but we don't know of a clean way to call the relevant async method to get the To-Dos when the viewmodel is created. 
* **Attempts To Fix:**
  - Looked through some potential solutions, like [this one](https://stackoverflow.com/questions/23048285/call-asynchronous-method-in-constructor). 
* **Solution:**
  - Call InitializeItems in the code-behind OnAppearing method, treating it like the OnInitializedAsync in Blazor.
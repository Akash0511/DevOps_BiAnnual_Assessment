pipeline{
    agent any
    environment{
        scannerHome = tool 'sonar_scanner_dotnet'
        registry = 'bhardwajakash/devops_assessment'
        docker_port = null
        username = 'akashbhardwaj'
        container_name = "c-${username}-${BRANCH_NAME}"
        container_exist = "${bat(script:"docker ps -a -q -f name=${env.container_name}",returnStdout:true).trim().readLines().drop(1).join("")}"
    }
    stages{
        stage('Checkout'){
            steps{
                echo "Checkout from git repository"
                checkout scm
                script{
                    if (BRANCH_NAME == 'master'){
                        docker_port = 7200
                    } else {
                        docker_port = 7400
                    }
                }
            }
        }
        stage('Build'){
            steps{
                echo "Build is running"
                bat "dotnet build"
            }
        }
        stage('Test cases'){
            steps{
                echo "Executing the test cases"
                bat "dotnet test"
            }
        }
        stage('Start SonarQube analysis'){
            steps{
                echo "start sonarqube analysis"
                withSonarQubeEnv('Test_Sonar'){
                    bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll begin /k:sonar-devops-akashbhardwaj /d:sonar.login=a1e7ade681b735fc04e3fe030a1385e78ba13bc7"
                    bat "dotnet clean"
                    bat "dotnet build"
                }
            }
        }
        stage('Stop SonarQube analysis'){
            steps{
                echo "stop sonarqube analysis"
                withSonarQubeEnv('Test_Sonar'){
                    bat "dotnet ${scannerHome}\\SonarScanner.MSBuild.dll end /d:sonar.login=a1e7ade681b735fc04e3fe030a1385e78ba13bc7"
                }
            }
        }
        stage('Docker Image'){
            steps{
                echo "Docker image build"
                bat "dotnet publish -c Release"
                bat "docker build -t i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} --no-cache -f Dockerfile ."
            }
        }
        stage('Push to Docker Hub'){
            steps{
                parallel(
                    "PrecontainerCheck":{
                        script{
                            echo "to check if ${env.container_name} already exist with container id = ${env.container_exist}"
                            if (env.container_exist != null){
                                echo "deleting existing ${env.container_name} container"
                                bat "docker stop ${env.container_name} && docker rm ${env.container_name}"
                            }
                        }
                    },
                    "Push to Docker Hub":{
                        script{
                            echo "Push to Docker Hub"
                            bat "docker tag i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} ${registry}:${BRANCH_NAME}-${BUILD_NUMBER}"
                            bat "docker tag i-${username}-${BRANCH_NAME}:${BUILD_NUMBER} ${registry}:latest-${BRANCH_NAME}"
                            withDockerRegistry([credentialsId:'DockerHub',url:""]){
                                bat "docker push ${registry}:${BRANCH_NAME}-${BUILD_NUMBER}"
                                bat "docker push ${registry}:latest-${BRANCH_NAME}"
                            }
                        }
                    }
                )                
            }
        }
        stage('Docker and Kubernetes deployment'){
            steps{
                parallel(
                    "Docker Deployment":{
                        script{
                            echo "Docker Deployment"
                            bat "docker run --name ${env.container_name} -d -p ${docker_port}:80 ${registry}:${BRANCH_NAME}-${BUILD_NUMBER}"
                        }
                    },
                    "Kubernetes Deployment":{
                        script{
                            echo "Kubernetes Deployment"
                            bat "kubectl apply -f deployment.yaml"
                            bat "kubectl apply -f service.yaml"
                        }
                    }
                )
            }
        }
    }
}
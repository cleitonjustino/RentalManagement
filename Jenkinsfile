pipeline {
    agent any
    tools {
        msbuild 'MSBuild 2022'
    }
    stages {
        stage('Checkout') {
            steps {
                // Clonar o repositório do Git
                //git 'https://github.com/cleitonjustino/RentalManagement.git'
                script {
                    // Checkout code without credentials
                    checkout([$class: 'GitSCM', branches: [[name: 'main']], userRemoteConfigs: [[url: 'https://github.com/cleitonjustino/RentalManagement.git']]])
                }
            }
        }
        
        stage('Restaurar Pacotes') {
            steps {
                // Restaurar os pacotes NuGet
                script {
                 // Grant execute permission to dotnet executable
                    sh 'chmod -R g+rwX /var/jenkins_home/tools/io.jenkins.plugins.dotnet.DotNetSDK/dotnet'

                    def dotnet = tool 'dotnet'
                    sh "${dotnet} restore"
                }
            }
        }
        
        stage('Compilar') {
            steps {
                // Compilar o projeto
                script {
                    def dotnet = tool 'dotnet'
                    sh "${dotnet} build --configuration Release"
                }
            }
        }
        
        stage('Testes') {
            steps {
                // Executar testes, se houver
                script {
                    def dotnet = tool 'dotnet'
                    sh "${dotnet} test --configuration Release"
                }
            }
        }
        
        stage('Publicar') {
            steps {
                // Publicar o projeto
                script {
                    def dotnet = tool 'dotnet'
                    sh "${dotnet} publish --configuration Release --output ./publish"
                }
            }
        }        
    }
    
    post {
        always {
            // Limpar após a conclusão (por exemplo, limpar artefatos temporários)
            deleteDir()
        }
    }
}

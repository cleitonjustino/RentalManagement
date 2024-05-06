pipeline {
    agent any
    tools {
        msbuild 'MSBuild 2022'
    }
    stages {
        stage('Checkout') {
            steps {
                // Clonar o repositório do Git
                git 'https://github.com/cleitonjustino/RentalManagement'
            }
        }
        
        stage('Restaurar Pacotes') {
            steps {
                // Restaurar os pacotes NuGet
                script {
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
        
        stage('Implantar') {
            steps {
                // Implantação da aplicação (por exemplo, copiá-la para o servidor)
                // Você pode adicionar os passos necessários para sua infraestrutura aqui
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

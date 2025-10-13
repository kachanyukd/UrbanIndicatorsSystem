Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/focal64"
  config.vm.provider "virtualbox"

  # Назва машини
  config.vm.hostname = "urban-traffic-sim"

  # Автоматичні команди після запуску
  config.vm.provision "shell", inline: <<-SHELL
    # Оновлення системи
    sudo apt-get update -y
    sudo apt-get install -y wget git

    # Встановлення .NET SDK
    wget https://download.visualstudio.microsoft.com/download/pr/d9f3c72f-6513-44f2-b04e-776a86b49c75/6d3c46cbfcaa6808d2dffba56f2922c0/dotnet-sdk-9.0.305-linux-x64.tar.gz
    mkdir -p $HOME/dotnet
    tar zxf dotnet-sdk-9.0.100-linux-x64.tar.gz -C $HOME/dotnet
    export DOTNET_ROOT=$HOME/dotnet
    export PATH=$PATH:$HOME/dotnet

    # Клонування репозиторію
    cd /home/vagrant
    git clone https://github.com/kachanyukd/UrbanIndicatorsSystem.git
    cd UrbanIndicatorsSystem

    # Побудова та запуск
    $HOME/dotnet/dotnet build
    nohup $HOME/dotnet/dotnet run --urls "http://0.0.0.0:5000" &
  SHELL

  # Переадресація порту
  config.vm.network "forwarded_port", guest: 5000, host: 5000
end
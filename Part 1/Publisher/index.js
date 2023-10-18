import net from "net"; // Importăm modulul "net" pentru a lucra cu socket-uri TCP.
import readline from "readline";

const brokerHost = '192.168.1.104';
const brokerPort = 5005;

const client = new net.Socket(); // Creăm un obiect de tip socket pentru a comunica cu broker-ul.

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

// Conectarea la broker.
client.connect(brokerPort, brokerHost, () => {
    console.log('Conectat la Broker');
    getUserInput();
});

// Gestionarea erorilor de conexiune.
client.on('error', (error) => {
    console.error('Eroare de conexiune:', error);
    rl.close();
});

// Funcția pentru a obține inputul utilizatorului pentru topic și mesaj.
function getUserInput() {
    rl.question('Introduceți topicul (sau tastați "exit" pentru a ieși): ', (topic) => {
        if (topic.toLowerCase() === 'exit') {
            // Închidem conexiunea și terminăm programul dacă utilizatorul tasteaza "exit".
            client.end();
            rl.close();
        } else {
            rl.question('Introduceți mesajul: ', (message) => {
                // Validăm inputul.
                if (topic.trim() === '' || message.trim() === '') {
                    console.error('Topicul și mesajul nu pot fi goale.');
                    getUserInput();
                    return;
                }

                const xmlData = `<Content><Topic>${topic}</Topic><Message>${message}</Message></Content>`;

                // Trimitem datele XML către broker.
                client.write(xmlData);

                // Obținem următorul input.
                getUserInput();
            });
        }
    });
}

// Gestionarea datelor primite de la broker.
client.on('data', (data) => {
    console.log('Răspuns primit de la Broker:', data.toString());
});

// Gestionarea deconectarii de la broker.
client.on('close', () => {
    console.log('Conexiunea cu Broker-ul s-a închis');
});

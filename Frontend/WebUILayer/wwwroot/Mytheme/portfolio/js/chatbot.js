let connection;
document.addEventListener('DOMContentLoaded', function () {
    initChatbot();
    initSignalR();
    loadSessionState();
});

function initSignalR() {
    const hubAdresi = window.AppConfig.ApiUrl + "/chatHub";
    connection = new signalR.HubConnectionBuilder().withUrl(hubAdresi).withAutomaticReconnect().build();
    connection.on("ReceiveBotMessage", function (response) {
        hideTypingIndicator();
        addMessage(response, 'bot');
        saveMessageToSession(response, 'bot');
    });
    connection.on("ReceiveError", function (errorMsg) {
        hideTypingIndicator();
        addMessage(errorMsg, 'bot');
    });
    connection.start().catch(err => console.error(err));
}

function initChatbot() {
    const toggle = document.querySelector('.chatbot-toggle');
    const chatbotWindow = document.querySelector('.chatbot-window');
    const closeBtn = document.querySelector('.chatbot-close');
    const input = document.querySelector('.chatbot-input input');
    const sendBtn = document.querySelector('.chatbot-send');
    const quickReplies = document.querySelectorAll('.quick-reply-btn');

    if (!toggle || !chatbotWindow) return;

    // --- SENİN SİLİNEN ESKİ KODLARIN (Chatbot Açma/Kapatma) ---
    toggle.addEventListener('click', function () {
        this.classList.toggle('active');
        chatbotWindow.classList.toggle('active');
        const icon = this.querySelector('i');

        if (chatbotWindow.classList.contains('active')) {
            icon.className = 'fas fa-times';
            localStorage.setItem('chatbotOpen', 'true');
        } else {
            icon.className = 'fas fa-comment-dots';
            localStorage.setItem('chatbotOpen', 'false');
        }
    });

    if (closeBtn) {
        closeBtn.addEventListener('click', function () {
            toggle.classList.remove('active');
            chatbotWindow.classList.remove('active');
            toggle.querySelector('i').className = 'fas fa-comment-dots';
            localStorage.setItem('chatbotOpen', 'false');
        });
    }

    // --- YENİ URL GÖNDEREN MESAJ FONKSİYONU ---
    function sendMessage() {
        const message = input?.value.trim();
        if (!message) return;

        addMessage(message, 'user');
        saveMessageToSession(message, 'user');
        input.value = '';
        showTypingIndicator();

        const currentUrl = window.location.pathname;

        connection.invoke("SendMessage", message, currentUrl).catch(err => {
            hideTypingIndicator();
            addMessage("Bağlantı hatası oluştu.", 'bot');
        });
    }

    if (sendBtn) sendBtn.addEventListener('click', sendMessage);
    if (input) input.addEventListener('keypress', function (e) { if (e.key === 'Enter') sendMessage(); });

    quickReplies.forEach(btn => {
        btn.addEventListener('click', function () {
            input.value = this.textContent.trim();
            sendMessage();
        });
    });
}

// --- GÜVENLİ XSS KORUMALI MESAJ EKLEME ---
function addMessage(text, sender) {
    const messagesContainer = document.querySelector('.chatbot-messages');
    const div = document.createElement('div');
    div.className = `chat-message ${sender}`;

    const avatarDiv = document.createElement('div');
    avatarDiv.className = 'message-avatar';
    avatarDiv.textContent = sender === 'bot' ? '🤖' : '👤';

    const contentDiv = document.createElement('div');
    contentDiv.className = 'message-content';
    contentDiv.textContent = text; // Güvenlik burada!

    div.appendChild(avatarDiv);
    div.appendChild(contentDiv);

    messagesContainer.appendChild(div);
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}

function showTypingIndicator() {
    const messagesContainer = document.querySelector('.chatbot-messages');
    const div = document.createElement('div');
    div.className = 'chat-message bot typing';
    div.innerHTML = `<div class="message-avatar">🤖</div><div class="typing-indicator"><span></span><span></span><span></span></div>`;
    messagesContainer.appendChild(div);
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}

function hideTypingIndicator() {
    const typing = document.querySelector('.typing');
    if (typing) typing.remove();
}

// --- ÇÖKMEYE KARŞI KORUMALI HAFIZA ---
function saveMessageToSession(text, sender) {
    try {
        let history = JSON.parse(localStorage.getItem('chatHistory')) || [];
        history.push({ text: text, sender: sender });
        localStorage.setItem('chatHistory', JSON.stringify(history));
    } catch (e) {
        console.error("Local storage kayıt hatası:", e);
        localStorage.removeItem('chatHistory');
    }
}

function loadSessionState() {
    try {
        if (localStorage.getItem('chatbotOpen') === 'true') {
            document.querySelector('.chatbot-toggle')?.classList.add('active');
            document.querySelector('.chatbot-window')?.classList.add('active');
            const icon = document.querySelector('.chatbot-toggle i');
            if (icon) icon.className = 'fas fa-times';
        }

        let history = JSON.parse(localStorage.getItem('chatHistory')) || [];
        if (history.length > 0) {
            const firstBotMessage = document.querySelector('.chatbot-messages .bot:first-child');
            if (firstBotMessage) firstBotMessage.remove();
            history.forEach(msg => addMessage(msg.text, msg.sender));
        }
    } catch (e) {
        console.error("Geçmiş mesajları yükleme hatası:", e);
        localStorage.removeItem('chatHistory');
    }
}

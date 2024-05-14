using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoVital.Models;
using EcoVital.Services;

namespace EcoVital.Views
{
    public partial class ChatBotPage : ContentPage
    {
        private readonly OpenAiService _openAiService = new();
        public List<Message> Messages { get; } = new();

        public ChatBotPage()
        {
            InitializeComponent();

            Messages.Add(new Message
            {
                Text = "Hola, bienvenido a la aplicación EcoVital, ¿qué puedo hacer por ti?", IsUserMessage = false
            });

            MessagesCollectionView.ItemsSource = Messages;
        }

        public ChatBotPage(OpenAiService openAiService)
        {
            InitializeComponent();
            _openAiService = openAiService;

            Messages.Add(new Message
            {
                Text = "Hola, bienvenido a la aplicación EcoVital, ¿qué puedo hacer por ti?",
                IsUserMessage = false
            });

            MessagesCollectionView.ItemsSource = Messages;
        }

        public async void OnSendClicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Se requiere conexión a Internet para usar el chatbot.", "OK");

                return;
            }

            var userInput = UserInput.Text;
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                Messages.Add(new Message { Text = userInput, IsUserMessage = true });
                RefreshMessagesCollectionView();

                var keywords = new List<string>
                {
                    "función", "haces", "sirves", "propósito", "puedes hacer", "capacidades",
                    "utilidad", "funcionalidades", "cómo ayudar", "qué ofreces", "servicios disponibles",
                    "acciones posibles", "tareas que puedes realizar", "para qué estás programado",
                    "qué problemas puedes solucionar", "cómo puedes asistir", "operaciones que puedes llevar a cabo",
                    "qué servicios provees", "capacidad de respuesta", "comandos disponibles",
                    "instrucciones que puedes seguir", "cómo interactuar contigo", "cuál es tu función"
                };

                if (keywords.Any(keyword => userInput.Trim().ToLower().Contains(keyword)))
                {
                    Messages.Add(new Message
                    {
                        Text = "Soy un asistente virtual diseñado para brindarte información acerca de la salud. " +
                               "Mi propósito es ofrecerte asesoramiento confiable, actualizado y accesible para tus " +
                               "consultas de salud. Puedo ayudarte a entender síntomas, darte información sobre tratamientos y medicamentos, " +
                               "sugerir hábitos saludables y ofrecerte guías de prevención de enfermedades. " +
                               "Estoy aquí para asistirte en tu búsqueda de una vida más saludable, " +
                               "respondiendo tus preguntas con información basada en evidencia y guiándote hacia las mejores prácticas de salud y bienestar. " +
                               "Sin embargo, es importante recordar que la información que ofrezco no reemplaza el consejo médico profesional, diagnóstico o tratamiento. " +
                               "Siempre debes consultar a un profesional de la salud calificado para asesoramiento específico sobre tu situación de salud.",
                        IsUserMessage = false
                    });
                }
                else
                {
                    var relevanceLevel = GetInputRelevanceLevel(userInput);

                    if (relevanceLevel == RelevanceLevel.High)
                    {
                        var response = await _openAiService.GetResponseAsync(userInput);
                        Messages.Add(new Message { Text = response, IsUserMessage = false });
                    }
                    else if (relevanceLevel == RelevanceLevel.Medium)
                    {
                        Messages.Add(new Message
                        {
                            Text =
                                "Parece que tu pregunta puede estar relacionada con el bienestar o la salud. ¿Podrías ser más específico?",
                            IsUserMessage = false
                        });
                    }
                    else
                    {
                        Messages.Add(new Message
                        {
                            Text = "Solo puedo responder preguntas relacionadas con el bienestar y la salud.",
                            IsUserMessage = false
                        });
                    }
                }

                RefreshMessagesCollectionView();
                UserInput.Text = string.Empty;
            }
        }

        private enum RelevanceLevel
        {
            High,
            Medium,
            Low
        }

        private RelevanceLevel GetInputRelevanceLevel(string userInput)
        {
            var normalizedInput = new string(userInput.ToLower().Where(c => !char.IsPunctuation(c)).ToArray());

            var greetingsAndGoodBye = new List<string>
            {
                "hola", "buenos días", "buenas tardes", "buenas noches",
                "hey", "hola hola", "qué tal", "qué onda", "cómo estás",
                "cómo va", "cómo te va", "cómo andas", "qué pasa", "buen día",
                "saludos", "qué hay", "hola qué tal", "buenas", "hi", "hello",
                "adios", "hasta luego", "hasta pronto", "nos vemos", "chao",
                "bye", "bye bye", "adiós", "hasta la próxima", "hasta mañana",
                "hasta la vista", "gracias", "gracias por tu ayuda", "gracias por la información",
                "gracias por responder", "gracias por tu tiempo", "si", "no"
            };

            if (greetingsAndGoodBye.Any(greeting => normalizedInput.Contains(greeting)))
            {
                return RelevanceLevel.High;
            }

            var keywordRelevance = new Dictionary<string, int>
            {
                { "salud", 3 }, { "bienestar", 3 }, { "ejercicio", 3 }, { "vida sana", 2 },
                { "meditación", 2 }, { "yoga", 2 }, { "fitness", 2 }, { "gimnasio", 2 },
                { "deporte", 2 }, { "actividad física", 2 }, { "rutina de ejercicio", 2 },
                { "rutina", 2 }, { "vivir", 2 }, { "vida", 2 }, { "dieta", 2 },
                { "nutrición", 2 }, { "alimentación saludable", 2 }, { "vegano", 2 },
                { "vegetariano", 2 }, { "proteínas", 2 }, { "carbohidratos", 2 },
                { "grasas saludables", 2 }, { "vitaminas", 2 }, { "minerales", 2 },
                { "hidratación", 2 }, { "agua", 2 }, { "medicamento", 2 }, { "dosis", 2 },
                { "tratamiento", 2 }, { "vacuna", 2 }, { "prevención", 2 }, { "cura", 2 },
                { "remedio natural", 2 }, { "suplemento", 2 }, { "estrés", 2 },
                { "ansiedad", 2 }, { "depresión", 2 }, { "bienestar emocional", 2 },
                { "psicología", 2 }, { "terapia", 2 }, { "mindfulness", 2 },
                { "chequeo", 2 }, { "examen médico", 2 }, { "salud cardiovascular", 2 },
                { "presión arterial", 2 }, { "colesterol", 2 }, { "glucosa", 2 },
                { "salud infantil", 2 }, { "salud de la mujer", 2 }, { "salud del hombre", 2 },
                { "embarazo", 2 }, { "tercera edad", 2 }, { "diabetes", 2 },
                { "obesidad", 2 }, { "hipertensión", 2 }, { "cáncer", 2 }, { "asma", 2 },
                { "artritis", 2 }, { "alergias", 2 }, { "sueño", 2 }, { "descanso", 2 },
                { "energía", 2 }, { "motivación", 2 }, { "objetivos de salud", 2 },
                { "hábitos saludables", 2 }, { "masaje", 2 }, { "rehabilitación", 2 },
                { "recuperación", 2 }, { "dolor", 2 }, { "caminata", 2 }, { "correr", 2 },
                { "natación", 2 }, { "ciclismo", 2 }, { "pilates", 2 }, { "baile", 2 },
                { "crossfit", 2 }, { "zumba", 2 }, { "senderismo", 2 }, { "escalada", 2 },
                { "aeróbicos", 2 }, { "entrenamiento de fuerza", 2 }, { "flexibilidad", 2 },
                { "movilidad", 2 }, { "entrenamiento funcional", 2 }, { "relajación", 2 },
                { "spa", 2 }, { "cuidado personal", 2 }, { "autocuidado", 2 },
                { "higiene", 2 }, { "belleza", 2 }, { "piel", 2 }, { "cuidado de la piel", 2 },
                { "hidratación de la piel", 2 }, { "anti-estrés", 2 }, { "detox", 2 },
                { "desintoxicación", 2 }, { "limpieza", 2 }, { "ayuno", 2 },
                { "ayuno intermitente", 2 }, { "keto", 2 }, { "cetogénica", 2 },
                { "paleo", 2 }, { "sin gluten", 2 }, { "vegetarianismo", 2 },
                { "crudiveganismo", 2 }, { "bajo en carbohidratos", 2 },
                { "dieta mediterránea", 2 }, { "superalimentos", 2 }, { "omega 3", 2 },
                { "antioxidantes", 2 }, { "fibra", 2 }, { "probióticos", 2 },
                { "prebióticos", 2 }, { "autoestima", 2 }, { "confianza", 2 },
                { "satisfacción personal", 2 }, { "felicidad", 2 }, { "resiliencia", 2 },
                { "gestión de la ira", 2 }, { "terapia cognitivo-conductual", 2 },
                { "psicoanálisis", 2 }, { "mindfulness avanzado", 2 }, { "acupuntura", 2 },
                { "homeopatía", 2 }, { "naturopatía", 2 }, { "reiki", 2 }, { "terapia floral", 2 },
                { "aromaterapia", 2 }, { "cromoterapia", 2 }, { "salud sexual", 2 },
                { "anticonceptivos", 2 }, { "fertilidad", 2 }, { "ITS", 2 },
                { "infecciones de transmisión sexual", 2 }, { "educación sexual", 2 },
                { "enfermedad cardiovascular", 2 }, { "enfermedad crónica", 2 },
                { "manejo del dolor", 2 }, { "tratamiento del cáncer", 2 },
                { "prevención del cáncer", 2 }, { "wearables", 2 }, { "aplicaciones de salud", 2 },
                { "tecnología wearable", 2 }, { "monitoreo de la salud", 2 },
                { "dispositivos de seguimiento", 2 }, { "ergonomía", 2 }, { "pausas activas", 2 },
                { "salud ocupacional", 2 }, { "bienestar laboral", 2 }, { "estrés laboral", 2 },
                { "calidad del aire", 2 }, { "contaminación", 2 }, { "espacios verdes", 2 },
                { "naturaleza", 2 }, { "biodiversidad", 2 }, { "vacunación", 2 },
                { "inmunización", 2 }, { "higiene mental", 2 }, { "sostenibilidad", 2 },
                { "vida ecológica", 2 }, { "productos ecológicos", 2 }, { "atención primaria", 2 },
                { "urgencias", 2 }, { "primeros auxilios", 2 }, { "cardiología", 2 },
                { "dermatología", 2 }, { "endocrinología", 2 }, { "gastroenterología", 2 },
                { "hematología", 2 }, { "neurología", 2 }, { "odontología", 2 },
                { "oftalmología", 2 }, { "ortopedia", 2 }, { "otorrinolaringología", 2 },
                { "pediatría", 2 }, { "psiquiatría", 2 }, { "reumatología", 2 },
                { "urología", 2 }, { "cuerpo y mente", 2 }, { "equilibrio mental", 2 },
                { "conciencia corporal", 2 }, { "epidemias", 2 }, { "pandemias", 2 },
                { "salud global", 2 }, { "comida orgánica", 2 }, { "alimentos integrales", 2 },
                { "dietas personalizadas", 2 }, { "marcha nórdica", 2 }, { "kayak", 2 },
                { "escalada deportiva", 2 }, { "patinaje", 2 }, { "equilibrio emocional", 2 },
                { "inteligencia emocional", 2 }, { "salud psicológica", 2 },
                { "vida minimalista", 2 }, { "reducción del estrés", 2 }, { "mejora del sueño", 2 },
                { "productos sostenibles", 2 }, { "huella de carbono", 2 },
                { "apps de meditación", 2 }, { "tecnología para el bienestar", 2 },
                { "pliometría", 2 }, { "HIIT", 2 }, { "tabata", 2 },
                { "salud holística", 2 }, { "bienestar integral", 2 }, { "hogar saludable", 2 },
                { "organización del hogar", 2 }, { "longevidad", 2 }, { "anti-envejecimiento", 2 },
                { "baños de bosque", 2 }, { "terapia con animales", 2 },
                { "suplementos vitamínicos", 2 }, { "nutrición deportiva", 2 },
                { "esclerosis múltiple", 2 }, { "enfermedad de Crohn", 2 },
                { "comunidad y bienestar", 2 }, { "voluntariado", 2 },
                { "mindfulness pleno", 2 }, { "gratitud", 2 }, { "diario de gratitud", 2 },
                { "biohacking", 2 }, { "crioterapia", 2 }, { "terapia de flotación", 2 },
                { "biofeedback", 2 }, { "ayurveda", 2 }, { "balance hormonal", 2 },
                { "detoxificación digital", 2 }, { "jejuar", 2 }, { "fasting", 2 },
                { "higiene del sueño", 2 }, { "sueño polifásico", 2 }, { "microbioma", 2 },
                { "salud intestinal", 2 }, { "fermentados", 2 }, { "kombucha", 2 },
                { "kefir", 2 }, { "alimentos fermentados", 2 }, { "entrenamiento de intervalos", 2 },
                { "calistenia", 2 }, { "parkour", 2 }, { "powerlifting", 2 },
                { "bodybuilding", 2 }, { "esgrima", 2 }, { "surf", 2 },
                { "escalada en roca", 2 }, { "esquí", 2 }, { "snowboarding", 2 },
                { "buceo", 2 }, { "paracaidismo", 2 }, { "triathlon", 2 },
                { "ironman", 2 }, { "maratón", 2 }, { "dietas flexibles", 2 },
                { "carga glucémica", 2 }, { "índice glucémico", 2 },
                { "alimentación consciente", 2 }, { "alimentos orgánicos", 2 },
                { "comida local", 2 }, { "huerto urbano", 2 }, { "alimentos kilómetro cero", 2 },
                { "alimentos de temporada", 2 }, { "comida real", 2 },
                { "resolución de conflictos", 2 }, { "terapia de pareja", 2 },
                { "coaching de vida", 2 }, { "gestión del cambio", 2 },
                { "terapia de aceptación y compromiso", 2 }, { "terapia narrativa", 2 },
                { "psicología positiva", 2 }, { "meditación guiada", 2 }, { "visualización", 2 },
                { "pruebas genéticas", 2 }, { "análisis de sangre avanzado", 2 },
                { "monitoreo de glucosa continuo", 2 }, { "pruebas de intolerancia alimentaria", 2 },
                { "screening de cáncer", 2 }, { "análisis de microbioma", 2 },
                { "gestión del tiempo", 2 }, { "productividad personal", 2 },
                { "ambiente de trabajo saludable", 2 }, { "cultura corporativa", 2 },
                { "mindfulness en el trabajo", 2 }, { "equilibrio trabajo-vida", 2 },
                { "vida sin desperdicio", 2 }, { "minimalismo", 2 },
                { "productos sin plástico", 2 }, { "moda sostenible", 2 },
                { "cosmética natural", 2 }, { "productos no tóxicos", 2 }, { "genómica", 2 },
                { "terapias con células madre", 2 }, { "medicina personalizada", 2 },
                { "realidad virtual para salud", 2 }, { "impresión 3D en medicina", 2 },
                { "inteligencia artificial en salud", 2 }, { "robots en cirugía", 2 },
                { "aceites esenciales", 2 }, { "suplementos nutricionales", 2 },
                { "hidroterapia", 2 }, { "musicoterapia", 2 }, { "arteterapia", 2 },
                { "jardinería terapéutica", 2 }, { "conexión con la naturaleza", 2 },
                { "fiebre", 2 }, { "tos", 2 }, { "dolor de cabeza", 2 },
                { "dolor de garganta", 2 }, { "dolor de estómago", 2 },
                { "dolor de espalda", 2 }, { "dolor de oído", 2 }, { "dolor de pecho", 2 },
                { "dolor de muelas", 2 }, { "dolor de piernas", 2 }, { "dolor de brazos", 2 },
                { "dolor de cuello", 2 }, { "dolor de rodilla", 2 }, { "dolor de cadera", 2 },
                { "dolor de hombro", 2 }, { "duele", 2 }, { "pica", 2 }, { "ardor", 2 },
                { "sangra", 2 }, { "inflamación", 2 }, { "hinchazón", 2 }, { "enrojecimiento", 2 },
                { "tengo", 2 }, { "padezco", 2 }, { "malestar", 2 }, { "molestia", 2 },
                { "incomodidad", 2 }, { "síntoma", 2 }, { "enfermedad", 2 },
            };

            int relevanceScore = 0;

            foreach (var keyword in keywordRelevance.Keys)
            {
                if (userInput.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    relevanceScore += keywordRelevance[keyword];
                }
            }

            if (relevanceScore > 2)
            {
                return RelevanceLevel.High;
            }

            if (relevanceScore > 0)
            {
                return RelevanceLevel.Medium;
            }

            return RelevanceLevel.Low;
        }

        void RefreshMessagesCollectionView()
        {
            MessagesCollectionView.ItemsSource = null;
            MessagesCollectionView.ItemsSource = Messages;
            MessagesCollectionView.ScrollTo(Messages.Count - 1, position: ScrollToPosition.End, animate: true);
        }
    }
}
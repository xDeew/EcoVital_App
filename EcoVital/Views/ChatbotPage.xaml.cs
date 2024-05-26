using EcoVital.Models;
using EcoVital.Services;
using System.Collections.Generic;
using System.Linq;

namespace EcoVital.Views;

public partial class ChatBotPage : ContentPage
{
    /// <summary>
    /// Página del chatbot que proporciona respuestas relacionadas con la salud.
    /// </summary>
    readonly OpenAiService _openAiService = new();

    readonly Dictionary<string, string> _healthResponses = new()
    {
        {
            "dolor de cabeza",
            "Para aliviar un dolor de cabeza:\n" +
            "1. Bebe mucha agua para mantenerte hidratado.\n" +
            "2. Descansa en un lugar oscuro y tranquilo.\n" +
            "3. Usa una compresa fría o caliente en la cabeza o el cuello.\n" +
            "4. Prueba técnicas de relajación como la meditación o la respiración profunda.\n" +
            "5. Toma un analgésico de venta libre si es necesario.\n" +
            "Si el dolor persiste o es muy intenso, es recomendable consultar a un médico. ¿Hay algo más específico sobre tu dolor de cabeza que te preocupe?"
        },
        {
            "dolor de garganta",
            "Para aliviar un dolor de garganta:\n" +
            "1. Haz gárgaras con agua salada varias veces al día.\n" +
            "2. Mantente hidratado bebiendo muchos líquidos, como agua, té o sopas.\n" +
            "3. Usa pastillas para la garganta o caramelos duros.\n" +
            "4. Evita fumar y las bebidas alcohólicas.\n" +
            "5. Usa un humidificador para mantener el aire húmedo.\n" +
            "Si tienes fiebre o el dolor es muy fuerte, consulta a un médico. ¿Presentas otros síntomas?"
        },
        {
            "dolor de estómago",
            "Para aliviar un dolor de estómago:\n" +
            "1. Bebe té de manzanilla para calmar el estómago.\n" +
            "2. Come alimentos suaves y fáciles de digerir, como arroz, plátanos y tostadas.\n" +
            "3. Evita comidas pesadas, grasas o picantes.\n" +
            "4. Aplica una compresa caliente en el abdomen.\n" +
            "5. Descansa y evita el estrés.\n" +
            "Si el dolor persiste o es muy intenso, consulta a un médico. ¿Tienes otros síntomas?"
        },
        {
            "estrés",
            "Para manejar el estrés:\n" +
            "1. Practica técnicas de relajación como la meditación, la respiración profunda o el yoga.\n" +
            "2. Haz ejercicio regularmente para liberar tensiones.\n" +
            "3. Mantén una dieta equilibrada y saludable.\n" +
            "4. Duerme lo suficiente cada noche.\n" +
            "5. Habla con un amigo o profesional de la salud mental sobre tus preocupaciones.\n" +
            "¿Hay algo específico que te cause estrés?"
        },
        {
            "ansiedad",
            "Para manejar la ansiedad:\n" +
            "1. Practica la meditación o el mindfulness para centrarte en el presente.\n" +
            "2. Realiza ejercicios de respiración profunda para calmarte.\n" +
            "3. Haz ejercicio regularmente para reducir la tensión.\n" +
            "4. Evita el consumo de cafeína y alcohol.\n" +
            "5. Considera hablar con un profesional de la salud mental para obtener apoyo adicional.\n" +
            "¿Puedes describir mejor cómo te sientes?"
        },
        {
            "depresión",
            "Para manejar la depresión:\n" +
            "1. Habla con un profesional de la salud mental para obtener apoyo y tratamiento.\n" +
            "2. Mantén una rutina diaria con actividades que disfrutes.\n" +
            "3. Haz ejercicio regularmente para mejorar tu estado de ánimo.\n" +
            "4. Come de manera saludable y equilibrada.\n" +
            "5. Duerme lo suficiente cada noche.\n" +
            "¿Hay algo en particular que quieras compartir?"
        },
        {
            "alergias",
            "Para manejar las alergias:\n" +
            "1. Evita los desencadenantes conocidos, como el polen, los ácaros del polvo o ciertos alimentos.\n" +
            "2. Mantén tu casa limpia y libre de polvo.\n" +
            "3. Usa purificadores de aire para reducir los alérgenos en el ambiente.\n" +
            "4. Toma medicamentos antihistamínicos si es necesario.\n" +
            "5. Consulta a un alergólogo para pruebas y tratamientos específicos.\n" +
            "¿Qué tipo de síntomas estás experimentando?"
        },
        {
            "diabetes",
            "Para manejar la diabetes:\n" +
            "1. Sigue un plan de alimentación saludable y equilibrada.\n" +
            "2. Haz ejercicio regularmente para mantener un peso saludable.\n" +
            "3. Controla tus niveles de azúcar en la sangre con regularidad.\n" +
            "4. Toma tus medicamentos según las indicaciones de tu médico.\n" +
            "5. Asiste a tus citas médicas y sigue las recomendaciones de tu equipo de salud.\n" +
            "¿Necesitas ayuda con algo específico relacionado con tu diabetes?"
        },
        {
            "obesidad",
            "Para manejar la obesidad:\n" +
            "1. Sigue un plan de alimentación saludable y bajo en calorías.\n" +
            "2. Incrementa tu actividad física diaria.\n" +
            "3. Consulta a un nutricionista para un plan personalizado.\n" +
            "4. Evita alimentos procesados y bebidas azucaradas.\n" +
            "5. Considera hablar con un médico sobre opciones adicionales como programas de pérdida de peso o cirugía bariátrica.\n" +
            "¿Tienes algún objetivo específico en mente?"
        },
        {
            "hipertensión",
            "Para manejar la hipertensión:\n" +
            "1. Sigue una dieta baja en sal y rica en frutas y verduras.\n" +
            "2. Haz ejercicio regularmente para mantener tu presión arterial bajo control.\n" +
            "3. Controla tu presión arterial en casa con un tensiómetro.\n" +
            "4. Toma tus medicamentos según las indicaciones de tu médico.\n" +
            "5. Reduce el estrés mediante técnicas de relajación como la meditación o el yoga.\n" +
            "¿Necesitas ayuda con alguna rutina o medicación?"
        },
        {
            "cáncer",
            "Para manejar un diagnóstico de cáncer:\n" +
            "1. Sigue el plan de tratamiento recomendado por tu médico.\n" +
            "2. Mantén una alimentación saludable para apoyar tu cuerpo durante el tratamiento.\n" +
            "3. Haz ejercicio moderado si te es posible.\n" +
            "4. Busca apoyo emocional a través de terapia, grupos de apoyo o seres queridos.\n" +
            "5. Infórmate sobre tu condición para tomar decisiones informadas sobre tu tratamiento.\n" +
            "¿Cómo te sientes al respecto?"
        },
        {
            "asma",
            "Para manejar el asma:\n" +
            "1. Evita los desencadenantes como el humo, el polvo y el polen.\n" +
            "2. Usa tu inhalador preventivo según las indicaciones del médico.\n" +
            "3. Lleva siempre contigo un inhalador de rescate.\n" +
            "4. Mantén un ambiente limpio y libre de alérgenos en casa.\n" +
            "5. Asiste a tus citas médicas para revisar tu plan de tratamiento.\n" +
            "¿Estás experimentando algún síntoma ahora mismo?"
        },
        {
            "artritis",
            "Para manejar la artritis:\n" +
            "1. Haz ejercicio regularmente para mantener la movilidad y fortalecer los músculos.\n" +
            "2. Mantén un peso saludable para reducir la presión en las articulaciones.\n" +
            "3. Usa calor o frío para aliviar el dolor y la inflamación.\n" +
            "4. Toma tus medicamentos según las indicaciones de tu médico.\n" +
            "5. Considera la fisioterapia para aprender ejercicios específicos que te ayuden.\n" +
            "¿Sientes dolor o rigidez en alguna articulación específica?"
        },
    };

    readonly List<string> _bannedWords = new()
    {
        "sicario", "asesino", "matar", "terrorista", "bomba", "arma", "violencia", "crimen", "droga", "secuestro",
        "extorsión", "robo", "violación", "abusar", "abusador", "abusadora", "abusivo", "abusiva", "abusar",
        "violador", "violadora", "violento", "violenta", "violencia", "asesinato", "asesinar", "matar", "muerte",
        "muerto", "muerta", "muertos", "muertas", "mueren", "murió", "mataron", "mata", "matan", "mató", "mató",
        "mataron", "mata", "matan", "mató", "mató", "mataron", "mata", "matan", "mató", "mató", "mataron", "mata",
        "matan", "mató", "mató", "mataron", "mata", "matan", "mató", "mató", "mataron", "mata", "matan", "mató",
    };

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ChatBotPage"/>.
    /// </summary>
    public ChatBotPage()
    {
        InitializeComponent();

        Messages.Add(new Message
        {
            Text = "Hola, bienvenido a la aplicación EcoVital, ¿qué puedo hacer por ti?", IsUserMessage = false
        });

        MessagesCollectionView.ItemsSource = Messages;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ChatBotPage"/> con un servicio de OpenAi proporcionado.
    /// </summary>
    /// <param name="openAiService">El servicio de OpenAi.</param>
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

    public List<Message> Messages { get; } = new();

    /// <summary>
    /// Maneja el evento de clic en el botón de enviar.
    /// </summary>
    /// <param name="sender">El origen del evento.</param>
    /// <param name="e">Los datos del evento.</param>
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

            if (ContainsBannedWords(userInput))
            {
                Messages.Add(new Message
                {
                    Text = "Lo siento, no puedo ayudarte con esa solicitud.",
                    IsUserMessage = false
                });
            }
            else
            {
                var response = GetHealthResponse(userInput);
                if (response != null)
                {
                    Messages.Add(new Message { Text = response, IsUserMessage = false });
                }
                else
                {
                    var relevanceLevel = GetInputRelevanceLevel(userInput);

                    if (relevanceLevel == RelevanceLevel.High)
                    {
                        var aiResponse = await _openAiService.GetResponseAsync(userInput);
                        Messages.Add(new Message { Text = aiResponse, IsUserMessage = false });
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
                            Text =
                                "Solo puedo responder preguntas relacionadas con el bienestar y la salud. ¿Te gustaría obtener más información sobre algún tema específico?",
                            IsUserMessage = false
                        });
                    }
                }
            }

            RefreshMessagesCollectionView();
            UserInput.Text = string.Empty;
        }
    }

    private bool ContainsBannedWords(string input)
    {
        foreach (var word in _bannedWords)
        {
            if (input.ToLower().Contains(word))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  Actualiza la colección de mensajes en la vista.
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns></returns>
    private string GetHealthResponse(string userInput)
    {
        foreach (var key in _healthResponses.Keys)
        {
            if (userInput.ToLower().Contains(key))
            {
                return _healthResponses[key];
            }
        }

        return null;
    }

    /// <summary>
    /// Obtiene el nivel de relevancia de la entrada del usuario.
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns></returns>
    RelevanceLevel GetInputRelevanceLevel(string userInput)
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

        if (greetingsAndGoodBye.Any(greeting => normalizedInput.Contains(greeting))) return RelevanceLevel.High;

        var keywordRelevance = new Dictionary<string, int>
        {
            { "salud", 5 }, { "bienestar", 5 }, { "ejercicio", 4 }, { "vida sana", 4 },
            { "meditación", 4 }, { "yoga", 4 }, { "fitness", 4 }, { "gimnasio", 4 },
            { "deporte", 4 }, { "actividad física", 4 }, { "rutina de ejercicio", 4 },
            { "rutina", 4 }, { "vivir", 3 }, { "vida", 3 }, { "dieta", 4 },
            { "nutrición", 4 }, { "alimentación saludable", 4 }, { "vegano", 4 },
            { "vegetariano", 4 }, { "proteínas", 4 }, { "carbohidratos", 4 },
            { "grasas saludables", 4 }, { "vitaminas", 4 }, { "minerales", 4 },
            { "hidratación", 4 }, { "agua", 4 }, { "medicamento", 4 }, { "dosis", 4 },
            { "tratamiento", 4 }, { "vacuna", 4 }, { "prevención", 4 }, { "cura", 4 },
            { "remedio natural", 4 }, { "suplemento", 4 }, { "estrés", 4 },
            { "ansiedad", 4 }, { "depresión", 4 }, { "bienestar emocional", 4 },
            { "psicología", 4 }, { "terapia", 4 }, { "mindfulness", 4 },
            { "chequeo", 4 }, { "examen médico", 4 }, { "salud cardiovascular", 4 },
            { "presión arterial", 4 }, { "colesterol", 4 }, { "glucosa", 4 },
            { "salud infantil", 4 }, { "salud de la mujer", 4 }, { "salud del hombre", 4 },
            { "embarazo", 4 }, { "tercera edad", 4 }, { "diabetes", 4 },
            { "obesidad", 4 }, { "hipertensión", 4 }, { "cáncer", 4 }, { "asma", 4 },
            { "artritis", 4 }, { "alergias", 4 }, { "sueño", 4 }, { "descanso", 4 },
            { "energía", 4 }, { "motivación", 4 }, { "objetivos de salud", 4 },
            { "hábitos saludables", 4 }, { "masaje", 4 }, { "rehabilitación", 4 },
            { "recuperación", 4 }, { "dolor", 4 }, { "caminata", 4 }, { "correr", 4 },
            { "natación", 4 }, { "ciclismo", 4 }, { "pilates", 4 }, { "baile", 4 },
            { "crossfit", 4 }, { "zumba", 4 }, { "senderismo", 4 }, { "escalada", 4 },
            { "aeróbicos", 4 }, { "entrenamiento de fuerza", 4 }, { "flexibilidad", 4 },
            { "movilidad", 4 }, { "entrenamiento funcional", 4 }, { "relajación", 4 },
            { "spa", 4 }, { "cuidado personal", 4 }, { "autocuidado", 4 },
            { "higiene", 4 }, { "belleza", 4 }, { "piel", 4 }, { "cuidado de la piel", 4 },
            { "hidratación de la piel", 4 }, { "anti-estrés", 4 }, { "detox", 4 },
            { "desintoxicación", 4 }, { "limpieza", 4 }, { "ayuno", 4 },
            { "ayuno intermitente", 4 }, { "keto", 4 }, { "cetogénica", 4 },
            { "paleo", 4 }, { "sin gluten", 4 }, { "vegetarianismo", 4 },
            { "crudiveganismo", 4 }, { "bajo en carbohidratos", 4 },
            { "dieta mediterránea", 4 }, { "superalimentos", 4 }, { "omega 3", 4 },
            { "antioxidantes", 4 }, { "fibra", 4 }, { "probióticos", 4 },
            { "prebióticos", 4 }, { "autoestima", 4 }, { "confianza", 4 },
            { "satisfacción personal", 4 }, { "felicidad", 4 }, { "resiliencia", 4 },
            { "gestión de la ira", 4 }, { "terapia cognitivo-conductual", 4 },
            { "psicoanálisis", 4 }, { "mindfulness avanzado", 4 }, { "acupuntura", 4 },
            { "homeopatía", 4 }, { "naturopatía", 4 }, { "reiki", 4 }, { "terapia floral", 4 },
            { "aromaterapia", 4 }, { "cromoterapia", 4 }, { "salud sexual", 4 },
            { "anticonceptivos", 4 }, { "fertilidad", 4 }, { "ITS", 4 },
            { "infecciones de transmisión sexual", 4 }, { "educación sexual", 4 },
            { "enfermedad cardiovascular", 4 }, { "enfermedad crónica", 4 },
            { "manejo del dolor", 4 }, { "tratamiento del cáncer", 4 },
            { "prevención del cáncer", 4 }, { "wearables", 4 }, { "aplicaciones de salud", 4 },
            { "tecnología wearable", 4 }, { "monitoreo de la salud", 4 },
            { "dispositivos de seguimiento", 4 }, { "ergonomía", 4 }, { "pausas activas", 4 },
            { "salud ocupacional", 4 }, { "bienestar laboral", 4 }, { "estrés laboral", 4 },
            { "calidad del aire", 4 }, { "contaminación", 4 }, { "espacios verdes", 4 },
            { "naturaleza", 4 }, { "biodiversidad", 4 }, { "vacunación", 4 },
            { "inmunización", 4 }, { "higiene mental", 4 }, { "sostenibilidad", 4 },
            { "vida ecológica", 4 }, { "productos ecológicos", 4 }, { "atención primaria", 4 },
            { "urgencias", 4 }, { "primeros auxilios", 4 }, { "cardiología", 4 },
            { "dermatología", 4 }, { "endocrinología", 4 }, { "gastroenterología", 4 },
            { "hematología", 4 }, { "neurología", 4 }, { "odontología", 4 },
            { "oftalmología", 4 }, { "ortopedia", 4 }, { "otorrinolaringología", 4 },
            { "pediatría", 4 }, { "psiquiatría", 4 }, { "reumatología", 4 },
            { "urología", 4 }, { "cuerpo y mente", 4 }, { "equilibrio mental", 4 },
            { "conciencia corporal", 4 }, { "epidemias", 4 }, { "pandemias", 4 },
            { "salud global", 4 }, { "comida orgánica", 4 }, { "alimentos integrales", 4 },
            { "dietas personalizadas", 4 }, { "marcha nórdica", 4 }, { "kayak", 4 },
            { "escalada deportiva", 4 }, { "patinaje", 4 }, { "equilibrio emocional", 4 },
            { "inteligencia emocional", 4 }, { "salud psicológica", 4 },
            { "vida minimalista", 4 }, { "reducción del estrés", 4 }, { "mejora del sueño", 4 },
            { "productos sostenibles", 4 }, { "huella de carbono", 4 },
            { "apps de meditación", 4 }, { "tecnología para el bienestar", 4 },
            { "pliometría", 4 }, { "HIIT", 4 }, { "tabata", 4 },
            { "salud holística", 4 }, { "bienestar integral", 4 }, { "hogar saludable", 4 },
            { "organización del hogar", 4 }, { "longevidad", 4 }, { "anti-envejecimiento", 4 },
            { "baños de bosque", 4 }, { "terapia con animales", 4 },
            { "suplementos vitamínicos", 4 }, { "nutrición deportiva", 4 },
            { "esclerosis múltiple", 4 }, { "enfermedad de Crohn", 4 },
            { "comunidad y bienestar", 4 }, { "voluntariado", 4 },
            { "mindfulness pleno", 4 }, { "gratitud", 4 }, { "diario de gratitud", 4 },
            { "biohacking", 4 }, { "crioterapia", 4 }, { "terapia de flotación", 4 },
            { "biofeedback", 4 }, { "ayurveda", 4 }, { "balance hormonal", 4 },
            { "detoxificación digital", 4 }, { "jejuar", 4 }, { "fasting", 4 },
            { "higiene del sueño", 4 }, { "sueño polifásico", 4 }, { "microbioma", 4 },
            { "salud intestinal", 4 }, { "fermentados", 4 }, { "kombucha", 4 },
            { "kefir", 4 }, { "alimentos fermentados", 4 }, { "entrenamiento de intervalos", 4 },
            { "calistenia", 4 }, { "parkour", 4 }, { "powerlifting", 4 },
            { "bodybuilding", 4 }, { "esgrima", 4 }, { "surf", 4 },
            { "escalada en roca", 4 }, { "esquí", 4 }, { "snowboarding", 4 },
            { "buceo", 4 }, { "paracaidismo", 4 }, { "triathlon", 4 }, { "ironman", 4 },
            { "maratón", 4 }, { "dietas flexibles", 4 }, { "carga glucémica", 4 },
            { "índice glucémico", 4 }, { "alimentación consciente", 4 },
            { "alimentos orgánicos", 4 }, { "comida local", 4 }, { "huerto urbano", 4 },
            { "alimentos kilómetro cero", 4 }, { "alimentos de temporada", 4 },
            { "comida real", 4 }, { "resolución de conflictos", 4 },
            { "terapia de pareja", 4 }, { "coaching de vida", 4 },
            { "gestión del cambio", 4 }, { "terapia de aceptación y compromiso", 4 },
            { "terapia narrativa", 4 }, { "psicología positiva", 4 },
            { "meditación guiada", 4 }, { "visualización", 4 }, { "pruebas genéticas", 4 },
            { "análisis de sangre avanzado", 4 }, { "monitoreo de glucosa continuo", 4 },
            { "pruebas de intolerancia alimentaria", 4 }, { "screening de cáncer", 4 },
            { "análisis de microbioma", 4 }, { "gestión del tiempo", 4 },
            { "productividad personal", 4 }, { "ambiente de trabajo saludable", 4 },
            { "cultura corporativa", 4 }, { "mindfulness en el trabajo", 4 },
            { "equilibrio trabajo-vida", 4 }, { "vida sin desperdicio", 4 },
            { "minimalismo", 4 }, { "productos sin plástico", 4 }, { "moda sostenible", 4 },
            { "cosmética natural", 4 }, { "productos no tóxicos", 4 }, { "genómica", 4 },
            { "terapias con células madre", 4 }, { "medicina personalizada", 4 },
            { "realidad virtual para salud", 4 }, { "impresión 3D en medicina", 4 },
            { "inteligencia artificial en salud", 4 }, { "robots en cirugía", 4 },
            { "aceites esenciales", 4 }, { "suplementos nutricionales", 4 },
            { "hidroterapia", 4 }, { "musicoterapia", 4 }, { "arteterapia", 4 },
            { "jardinería terapéutica", 4 }, { "conexión con la naturaleza", 4 },
            { "fiebre", 4 }, { "tos", 4 }, { "dolor de cabeza", 4 },
            { "dolor de garganta", 4 }, { "dolor de estómago", 4 },
            { "dolor de espalda", 4 }, { "dolor de oído", 4 }, { "dolor de pecho", 4 },
            { "dolor de muelas", 4 }, { "dolor de piernas", 4 }, { "dolor de brazos", 4 },
            { "dolor de cuello", 4 }, { "dolor de rodilla", 4 }, { "dolor de cadera", 4 },
            { "dolor de hombro", 4 }, { "duele", 4 }, { "pica", 4 }, { "ardor", 4 },
            { "sangra", 4 }, { "inflamación", 4 }, { "hinchazón", 4 }, { "enrojecimiento", 4 },
            { "tengo", 4 }, { "padezco", 4 }, { "malestar", 4 }, { "molestia", 4 },
            { "incomodidad", 4 }, { "síntoma", 4 }, { "enfermedad", 4 }
        };

        var relevanceScore = 0;

        foreach (var keyword in keywordRelevance.Keys)
            if (normalizedInput.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                relevanceScore += keywordRelevance[keyword];

        if (relevanceScore > 4) return RelevanceLevel.High;

        if (relevanceScore > 2) return RelevanceLevel.Medium;

        return RelevanceLevel.Low;
    }

    /// <summary>
    ///  Actualiza la colección de mensajes en la vista.
    /// </summary>
    void RefreshMessagesCollectionView()
    {
        MessagesCollectionView.ItemsSource = null;
        MessagesCollectionView.ItemsSource = Messages;
        MessagesCollectionView.ScrollTo(Messages.Count - 1, position: ScrollToPosition.End, animate: true);
    }

    /// <summary>
    ///  Nivel de relevancia de la entrada del usuario.
    /// </summary>
    enum RelevanceLevel
    {
        High,
        Medium,
        Low
    }
}
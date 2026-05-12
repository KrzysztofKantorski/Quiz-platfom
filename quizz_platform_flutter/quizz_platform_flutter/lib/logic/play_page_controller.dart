import 'dart:async';
import 'package:flutter_riverpod/legacy.dart';
import '../../data/models/question.dart';

// --- 1. STATE ---
class QuizGameState {
  final bool isStarted;
  final bool isFinished;
  final Duration duration;
  final Map<int, int?> userAnswers; // indeks pytania -> indeks odpowiedzi

  QuizGameState({
    this.isStarted = false,
    this.isFinished = false,
    this.duration = const Duration(),
    this.userAnswers = const {},
  });

  QuizGameState copyWith({
    bool? isStarted,
    bool? isFinished,
    Duration? duration,
    Map<int, int?>? userAnswers,
  }) {
    return QuizGameState(
      isStarted: isStarted ?? this.isStarted,
      isFinished: isFinished ?? this.isFinished,
      duration: duration ?? this.duration,
      userAnswers: userAnswers ?? this.userAnswers,
    );
  }
}

// --- 2. CONTROLLER ---
class QuizGameController extends StateNotifier<QuizGameState> {
  QuizGameController() : super(QuizGameState());

  Timer? _timer;

  void startQuiz() {
    _timer?.cancel();
    state = QuizGameState(isStarted: true); // Reset i start
    _timer = Timer.periodic(const Duration(seconds: 1), (timer) {
      state = state.copyWith(duration: state.duration + const Duration(seconds: 1));
    });
  }

  void updateAnswer(int questionIdx, int answerIdx) {
    if (state.isFinished) return;
    
    final newAnswers = Map<int, int?>.from(state.userAnswers);
    newAnswers[questionIdx] = answerIdx;
    state = state.copyWith(userAnswers: newAnswers);
  }

  void finishQuiz() {
    _timer?.cancel();
    state = state.copyWith(isFinished: true);
  }

  // Obliczanie punktów
  int calculateScore(List<Question> questions) {
    int score = 0;
    state.userAnswers.forEach((qIdx, aIdx) {
      if (aIdx != null && 
          qIdx < questions.length && 
          aIdx < questions[qIdx].answers.length &&
          questions[qIdx].answers[aIdx].isCorrect) {
        score += questions[qIdx].points;
      }
    });
    return score;
  }

  // Pomocnik do formatowania czasu
  String get formattedDuration {
    String twoDigits(int n) => n.toString().padLeft(2, "0");
    final d = state.duration;
    return "${twoDigits(d.inMinutes)}:${twoDigits(d.inSeconds.remainder(60))}";
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }
}
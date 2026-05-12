import 'package:flutter_riverpod/legacy.dart';
import '../../data/datasources/api_handler.dart';
import '../../data/models/quiz.dart';
import '../../data/models/question.dart';

// --- 1. STATE  ---
class BuildQuizState {
  final bool isLoading;
  final List<Question> questions;
  final String title;
  final String description;

  BuildQuizState({
    this.isLoading = false,
    this.questions = const [],
    this.title = '',
    this.description = '',
  });

  BuildQuizState copyWith({
    bool? isLoading,
    List<Question>? questions,
    String? title,
    String? description,
  }) {
    return BuildQuizState(
      isLoading: isLoading ?? this.isLoading,
      questions: questions ?? this.questions,
      title: title ?? this.title,
      description: description ?? this.description,
    );
  }
}

// --- 2. CONTROLLER  ---
class BuildQuizController extends StateNotifier<BuildQuizState> {
  final ApiHandler _api;

  BuildQuizController(this._api) : super(BuildQuizState());

  // Inicjalizacja danych w trybie edycji
  Future<void> loadQuizData(int quizId) async {
    state = state.copyWith(isLoading: true);
    try {
      final quiz = await _api.getFullQuiz(quizId);
      final questions = await _api.getQuestionsForQuiz(quizId);
      if (quiz != null) {
        state = state.copyWith(
          title: quiz.title,
          description: quiz.description,
          questions: questions,
        );
      }
    } finally {
      state = state.copyWith(isLoading: false);
    }
  }

  // Zarządzanie listą pytań (lokalnie)
  void addOrUpdateQuestion(Question question, int? index) {
    final newList = List<Question>.from(state.questions);
    if (index == null) {
      newList.add(question);
    } else {
      newList[index] = question;
    }
    state = state.copyWith(questions: newList);
  }

  // Usuwanie pytania (z API i z listy lokalnej)
  Future<bool> deleteQuestion(int index) async {
    final question = state.questions[index];
    if (question.id != null) {
      final success = await _api.deleteQuestion(question.id!);
      if (!success) return false;
    }
    final newList = List<Question>.from(state.questions)..removeAt(index);
    state = state.copyWith(questions: newList);
    return true;
  }

  // Zapis całego quizu
  Future<bool> saveFullQuiz({
    int? editId, 
    required String title, 
    required String description
  }) async {
    state = state.copyWith(isLoading: true);
    try {
      int? currentQuizId = editId;
      bool success = false;

      // 1. Zapis/Aktualizacja nagłówka
      if (editId == null) {
        currentQuizId = await _api.postQuiz(Quiz(title: title, description: description));
        success = currentQuizId != null;
      } else {
        success = await _api.updateQuiz(Quiz(id: editId, title: title, description: description));
      }

      if (!success || currentQuizId == null) return false;

      // 2. Synchronizacja pytań
      for (var q in state.questions) {
        if (q.id == null) {
          await _api.addQuestionToQuiz(currentQuizId, q);
        } else {
          await _api.updateQuestion(q);
        }
      }
      return true;
    } finally {
      state = state.copyWith(isLoading: false);
    }
  }
}
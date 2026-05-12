import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_riverpod/legacy.dart';
import 'package:quizz_platform_flutter/data/datasources/api_handler.dart';
import 'package:quizz_platform_flutter/data/models/question.dart';
import 'package:quizz_platform_flutter/data/models/quiz.dart';
import 'package:quizz_platform_flutter/logic/build_page_controller.dart';
import 'package:quizz_platform_flutter/logic/main_page_controller.dart';
import 'package:quizz_platform_flutter/logic/play_page_controller.dart';

// 1. Instancja API - dostępna w całej aplikacji
final apiHandlerProvider = Provider((ref) => ApiHandler());

// 2. Lista quizów pobierana z serwera
final quizzesProvider = FutureProvider.autoDispose<List<Quiz>>((ref) async {
  final apiHandler = ref.watch(apiHandlerProvider);
  return await apiHandler.getQuizData();
});

// 3. Obecnie wybrany quiz 
final selectedQuizProvider = StateProvider<Quiz?>((ref) => null);

// 4. Tymczasowa lista pytań
final tempQuestionsProvider = StateProvider<List<Question>>((ref) => []);

// 5. Provider dla konkretnych pytań wybranego quizu
final quizQuestionsProvider = FutureProvider.family<List<Question>, int>((ref, quizId) async {
  final apiHandler = ref.watch(apiHandlerProvider);
  return await apiHandler.getQuestionsForQuiz(quizId);
});
// 6. Provider dla głównej strony
final mainPageControllerProvider = Provider((ref) {
  return MainPageController(ref);
});
// 7. Provider dla strony edycji
final buildQuizProvider = StateNotifierProvider.autoDispose<BuildQuizController, BuildQuizState>((ref) {
  return BuildQuizController(ref.watch(apiHandlerProvider));
});
// 8. Provider dla strony rozgrywki
final quizGameProvider = StateNotifierProvider.autoDispose<QuizGameController, QuizGameState>((ref) {
  return QuizGameController();
});
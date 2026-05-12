import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:quizz_platform_flutter/presentation/pages/play_page.dart';
import 'package:quizz_platform_flutter/presentation/services/ui_service.dart';
import '../../providers/quiz_providers.dart';

class MainPageController {
  final Ref ref;
  MainPageController(this.ref);

  Future<void> handlePlayQuiz(BuildContext context, dynamic quiz) async {
    UIService.showLoadingDialog(context);
    try {
      final questions = await ref.read(apiHandlerProvider).getQuestionsForQuiz(quiz.id);
      if (context.mounted) Navigator.pop(context); // Zamknij loader

      if (questions.isNotEmpty) {
        Navigator.push(context, MaterialPageRoute(
          builder: (context) => PlayPage(quiz: quiz, questions: questions)));
      } else {
        UIService.showErrorSnackBar(context, "Ten quiz nie ma pytań!");
      }
    } catch (e) {
      if (context.mounted) Navigator.pop(context);
      UIService.showErrorSnackBar(context, "Błąd pobierania: $e");
    }
  }

  Future<void> handleDelete(BuildContext context, int id) async {
    final success = await ref.read(apiHandlerProvider).deleteQuiz(id);
    if (success) {
      ref.invalidate(quizzesProvider);
      UIService.showSuccessSnackBar(context, "Quiz usunięty");
    } else {
      UIService.showErrorSnackBar(context, "Nie udało się usunąć quizu");
    }
  }
}

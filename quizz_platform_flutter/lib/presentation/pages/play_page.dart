import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:quizz_platform_flutter/presentation/services/ui_service.dart';
import 'package:quizz_platform_flutter/providers/quiz_providers.dart';
import 'package:quizz_platform_flutter/styles/widgets/play_page_widgets.dart';
import '../../data/models/quiz.dart';
import '../../data/models/question.dart';
import '../../styles/app_colors.dart';

class PlayPage extends ConsumerWidget {
  final Quiz quiz;
  final List<Question> questions;

  const PlayPage({super.key, required this.quiz, required this.questions});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final state = ref.watch(quizGameProvider);
    final notifier = ref.read(quizGameProvider.notifier);

    return Scaffold(
      backgroundColor: AppColors.primaryLight,
      appBar: AppBar(
        title: Text(quiz.title, style: const TextStyle(color: AppColors.accent)),
        backgroundColor: AppColors.primary,
        elevation: 0,
      ),
      body: Column(
        children: [
          QuizHeader(
            formattedTime: notifier.formattedDuration,
            answeredCount: state.userAnswers.length,
            totalCount: questions.length,
            isStarted: state.isStarted,
            isFinished: state.isFinished,
            onStart: () => notifier.startQuiz(),
            onStop: () {
              notifier.finishQuiz();
              // WYWOŁANIE SERWISU UI
              UIService.showResultDialog(
                context: context,
                score: notifier.calculateScore(questions),
                maxScore: questions.fold(0, (sum, q) => sum + q.points),
                onReview: () {}, // Zamyka dialog i pozwala patrzeć na listę
                onExit: () {
                  Navigator.pop(context); // Powrót z dialogu
                  Navigator.pop(context); // Powrót do MainPage
                },
              );
            },
          ),
          Expanded(
            child: Opacity(
              opacity: state.isStarted ? 1.0 : 0.0,
              child: ListView.builder(
                padding: const EdgeInsets.all(16),
                itemCount: questions.length,
                itemBuilder: (context, index) => QuestionCard(
                  index: index,
                  question: questions[index],
                  gameState: state,
                  onAnswerSelected: (aIdx) => notifier.updateAnswer(index, aIdx),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
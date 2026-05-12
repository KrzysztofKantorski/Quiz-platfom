import 'package:flutter/material.dart';
import 'package:quizz_platform_flutter/logic/play_page_controller.dart';
import '../../data/models/question.dart';
import '../../styles/app_colors.dart';

// WIDGET 1: Nagłówek z zegarem i przyciskami
class QuizHeader extends StatelessWidget {
  final String formattedTime;
  final int answeredCount;
  final int totalCount;
  final bool isStarted;
  final bool isFinished;
  final VoidCallback onStart;
  final VoidCallback onStop;

  const QuizHeader({
    super.key,
    required this.formattedTime,
    required this.answeredCount,
    required this.totalCount,
    required this.isStarted,
    required this.isFinished,
    required this.onStart,
    required this.onStop,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(16),
      color: AppColors.primary,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text("Czas: $formattedTime", 
                   style: const TextStyle(color: AppColors.accent, fontSize: 20, fontWeight: FontWeight.bold)),
              Text("Postęp: $answeredCount/$totalCount", 
                   style: const TextStyle(color: AppColors.textSecondary)),
            ],
          ),
          Row(
            children: [
              ElevatedButton(
                onPressed: (!isStarted || isFinished) ? onStart : null,
                style: ElevatedButton.styleFrom(backgroundColor: AppColors.success),
                child: const Text("START"),
              ),
              const SizedBox(width: 8),
              ElevatedButton(
                onPressed: (isStarted && !isFinished) ? onStop : null,
                style: ElevatedButton.styleFrom(backgroundColor: AppColors.error),
                child: const Text("STOP"),
              ),
            ],
          )
        ],
      ),
    );
  }
}

// WIDGET 2: Karta z pytaniem
class QuestionCard extends StatelessWidget {
  final int index;
  final Question question;
  final QuizGameState gameState;
  final Function(int) onAnswerSelected;

  const QuestionCard({
    super.key,
    required this.index,
    required this.question,
    required this.gameState,
    required this.onAnswerSelected,
  });
@override
  Widget build(BuildContext context) {
    return Card(
      color: AppColors.primary.withValues(alpha: 0.5),
      margin: const EdgeInsets.only(bottom: 16),
      child: Padding(
        padding: const EdgeInsets.all(12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text("${index + 1}. ${question.text}", 
                 style: const TextStyle(color: Colors.white, fontSize: 18, fontWeight: FontWeight.bold)),
            const SizedBox(height: 12),
            RadioGroup<int>(
              groupValue: gameState.userAnswers[index], 
              onChanged: gameState.isFinished ? (_){} : (val) => onAnswerSelected(val!),
              child: Column(
                children: question.answers.asMap().entries.map((entry) {
                  final aIdx = entry.key;
                  final answer = entry.value;

                  Color? bgColor;
                  if (gameState.isFinished) {
                    if (answer.isCorrect) {
                      bgColor = AppColors.success.withValues(alpha: 0.2);
                    } else if (gameState.userAnswers[index] == aIdx) {
                      bgColor = AppColors.error.withValues(alpha: 0.2);
                    }
                  }

                  return Container(
                    margin: const EdgeInsets.only(bottom: 4), 
                    decoration: BoxDecoration(
                      color: bgColor,
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: RadioListTile<int>(
                      title: Text(answer.text, style: const TextStyle(color: Colors.white70)),
                      value: aIdx,
                      activeColor: AppColors.accent,
                      contentPadding: EdgeInsets.zero,
                    ),
                  );
                }).toList(),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
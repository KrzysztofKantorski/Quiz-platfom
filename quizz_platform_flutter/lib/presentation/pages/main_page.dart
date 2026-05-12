import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:quizz_platform_flutter/presentation/services/ui_service.dart';
import 'package:quizz_platform_flutter/styles/widgets/main_page_widgets.dart';
import '../../providers/quiz_providers.dart';
import '../../styles/app_colors.dart';
import 'build_quiz_page.dart';

class MainPage extends ConsumerWidget {
  const MainPage({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final quizzesAsync = ref.watch(quizzesProvider);
    final selectedQuiz = ref.watch(selectedQuizProvider);
    final controller = ref.read(mainPageControllerProvider);

    return Scaffold(
      backgroundColor: AppColors.primaryLight,
      body: Center(
        child: Container(
          constraints: const BoxConstraints(maxWidth: 800),
          margin: const EdgeInsets.symmetric(horizontal: 20, vertical: 40),
          padding: const EdgeInsets.all(30),
          decoration: BoxDecoration(
            color: AppColors.primary,
            borderRadius: BorderRadius.circular(25),
            boxShadow: [BoxShadow(color: Colors.black.withValues(alpha: 0.4), blurRadius: 20)],
          ),
          child: quizzesAsync.when(
            data: (quizzes) => Column(
              children: [
                MainHeader(count: quizzes.length, onRefresh: () => ref.refresh(quizzesProvider)),
                const Divider(color: AppColors.accent, height: 40),
                Expanded(
                  child: quizzes.isEmpty 
                    ? const EmptyQuizPlaceholder()
                    : ListView.builder(
                        itemCount: quizzes.length,
                        itemBuilder: (context, index) {
                          final quiz = quizzes[index];
                          return QuizListCard(
                            quiz: quiz,
                            isSelected: selectedQuiz?.id == quiz.id,
                            onTap: () => ref.read(selectedQuizProvider.notifier).state = quiz,
                            onPlay: () => controller.handlePlayQuiz(context, quiz),
                            onEdit: () => Navigator.push(context, MaterialPageRoute(
                              builder: (context) => BuildQuizPage(editQuizId: quiz.id))),
                            onDelete: () => UIService.showConfirmDeleteDialog(
                              context: context, 
                              onConfirm: () => controller.handleDelete(context, quiz.id!)
                            ),
                          );
                        },
                      ),
                ),
              ],
            ),
            loading: () => const Center(child: CircularProgressIndicator(color: AppColors.accent)),
            error: (e, _) => Center(child: Text("Błąd: $e", style: const TextStyle(color: AppColors.error))),
          ),
        ),
      ),
    );
  }
}
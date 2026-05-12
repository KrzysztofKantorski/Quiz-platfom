import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:quizz_platform_flutter/presentation/services/ui_service.dart';
import 'package:quizz_platform_flutter/providers/quiz_providers.dart';
import 'package:quizz_platform_flutter/styles/widgets/build_page_widgets.dart';
import '../../styles/app_colors.dart';

class BuildQuizPage extends ConsumerStatefulWidget {
  final int? editQuizId;
  const BuildQuizPage({super.key, this.editQuizId});

  @override
  ConsumerState<BuildQuizPage> createState() => _BuildQuizPageState();
}

class _BuildQuizPageState extends ConsumerState<BuildQuizPage> {
  final titleController = TextEditingController();
  final descController = TextEditingController();

  @override
  void initState() {
    super.initState();
    if (widget.editQuizId != null) {
      // Ładujemy dane asynchronicznie przez notifier
      Future.microtask(() => 
        ref.read(buildQuizProvider.notifier).loadQuizData(widget.editQuizId!)
      ).then((_) {
        final state = ref.read(buildQuizProvider);
        titleController.text = state.title;
        descController.text = state.description;
      });
    }
  }

  void _handleSave() async {
    final success = await ref.read(buildQuizProvider.notifier).saveFullQuiz(
      editId: widget.editQuizId,
      title: titleController.text,
      description: descController.text,
    );

    if (success) {
      Navigator.pop(context);
      UIService.showSuccessSnackBar(context, "Quiz zapisany!");
    } else {
      UIService.showErrorSnackBar(context, "Błąd zapisu.");
    }
  }

  @override
  Widget build(BuildContext context) {
    final state = ref.watch(buildQuizProvider);
    final notifier = ref.read(buildQuizProvider.notifier);

    if (state.isLoading) return const Scaffold(body: Center(child: CircularProgressIndicator()));

    return Scaffold(
      backgroundColor: AppColors.primaryLight,
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: Row(
          children: [
            // LEWA STRONA: FORMULARZ
            Expanded(
              child: Column(
                children: [
                  BuildTextField(hint: "Tytuł", controller: titleController),
                  const SizedBox(height: 20),
                  BuildTextField(hint: "Opis", controller: descController, maxLines: 5),
                  const Spacer(),
                  ElevatedButton(onPressed: _handleSave, child: const Text("ZAPISZ QUIZ")),
                ],
              ),
            ),
            const SizedBox(width: 40),
            // PRAWA STRONA: LISTA PYTAŃ
            Expanded(
              child: Column(
                children: [
                  IconButton.filled(
                    style: IconButton.styleFrom(
                      backgroundColor: AppColors.success,
                      foregroundColor: AppColors.primaryLight
                    ),
                    onPressed: () => UIService.showQuestionDialog(
                      context: context, 
                      onSave: (q) => notifier.addOrUpdateQuestion(q, null),
                    ),
                    icon: const Icon(Icons.add),
                  ),
                  Expanded(
                    child: ListView.builder(
                      itemCount: state.questions.length,
                      itemBuilder: (context, i) => QuestionListTile(
                        text: state.questions[i].text,
                        points: state.questions[i].points,
                        onEdit: () => UIService.showQuestionDialog(
                          context: context,
                          existingQuestion: state.questions[i],
                          onSave: (q) => notifier.addOrUpdateQuestion(q, i),
                        ),
                        onDelete: () => notifier.deleteQuestion(i),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
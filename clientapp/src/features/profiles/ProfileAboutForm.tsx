import { Formik, Form, ErrorMessage } from "formik";
import React from "react";
import { Button } from "semantic-ui-react";
import MyTextArea from "../../app/common/form/MyTextArea";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Profile } from "../../app/models/profile";
import * as Yup from "yup";
import ValidationErrors from "../errors/ValidationErrors";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";

interface Props {
	profile: Profile;
	closeEditMode: () => void;
}

export default observer(function ProfileAboutForm({ profile, closeEditMode }: Props) {
	const {profileStore} = useStore();

	return (
		<Formik
			initialValues={{
				displayName: profile.displayName,
				bio: profile.bio ?? "",
				error: null
			}}
			onSubmit={(values, {setErrors, setSubmitting}) => {
				profileStore.updateProfile({...values, username: profile.username})
				.then(_ => 
					{
						setSubmitting(false);
						closeEditMode();
					}) 
				.catch(errors => 
					{
						setErrors(errors);
					});
			}}
			validationSchema={Yup.object({
				displayName: Yup.string().required(
					"Display name is a required field"
				),
			})}
		>
			{({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
				<Form className="ui form error" onSubmit={handleSubmit} autoComplete="off">
					<MyTextInput
						name="displayName"
						placeholder="Display Name"
					/>
					<MyTextArea
						name="bio"
						placeholder="Add your bio"
						rows={3}
					/>

					<ErrorMessage
						name="error"
						render={() => (
							<ValidationErrors errors={errors.error} />
						)}
					/>

					<Button
						disabled={!isValid || !dirty || isSubmitting}
						loading={isSubmitting}
						positive
						content="Update profile"
						type="submit"
						floated="right"
					/>
				</Form>
			)}
		</Formik>
	);
})
